using Assets.TValle.Tools.Runtime.Characters;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff;
using Assets.TValle.Tools.Runtime.Characters.Intections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Scenes.BuffAndDebuff
{
    public static class DefaultBuffAndDebuffGenerator
    {
        public static Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffForEmotionForCharacter> GenerateEmotionForCharacterBuffBySceneInteractions(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to)
        {
            var r = new Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffForEmotionForCharacter>();
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);

            EmotionInteraction(archivedInteractions, Emotion.pleasure, false, out Interaction pleasureInter);
            EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);


            var favorabilityDefaultValue = 0f;

            if(pleasureInter.isValid)
                favorabilityDefaultValue += (pleasureInter.damagePercentage / 100f) * pleasureInter.damageScore;
            if(orgasmInter.isValid)
                favorabilityDefaultValue += orgasmInter.times;



            foreach(Emotion emo in EmotionExt.emotionsWithDefaultValueBuff)
            {
                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);
                EmotionInteraction(archivedInteractions, emo, true, out Interaction maxValueInter);

                if(maxValueInter.isValid && maxValueInter.times > 0)
                {
                    BuffForEmotionForCharacter buffByMaxValueReachedPermanent = new BuffForEmotionForCharacter()//Ex: for when she is angry at a character couse max rage was reached | PERMANENT
                    {
                        characterID = from.ID,
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = emo == Emotion.pleasure ? 1f/*times already is greater that one*/ + ((float)(maxValueInter.times - 1) * 0.5f) : maxValueInter.times,
                    };
                    r.Add(buffByMaxValueReachedPermanent.valueId, buffByMaxValueReachedPermanent);

                    BuffForEmotionForCharacter buffByMaxValueReachedTemporal = buffByMaxValueReachedPermanent;//Ex: for when she is angry at a character couse max rage was reached | TEMPORAL
                    buffByMaxValueReachedTemporal.value *= 5f;
                    buffByMaxValueReachedTemporal.durationInDays = 1;
                    r.Add(buffByMaxValueReachedTemporal.valueId, buffByMaxValueReachedTemporal);
                }
                else
                {
                    BuffForEmotionForCharacter buffByMaxValueNotReachedPermanent = new BuffForEmotionForCharacter()//Ex: if she was not angry to the max, then she has a change to calm down | PERMANENT
                    {
                        characterID = from.ID,
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = -(1f - Mathf.Clamp01(inter.damagePercentage / 100f)),
                    };
                    r.Add(buffByMaxValueNotReachedPermanent.valueId, buffByMaxValueNotReachedPermanent);
                }

                BuffForEmotionForCharacter buffForDamageDoneTemporal = new BuffForEmotionForCharacter()//Ex: for when she is angry at a character and she has not calm down | TEMPORAL
                {
                    characterID = from.ID,
                    emotion = emo,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = 1,
                    value = Mathf.Clamp(inter.damagePercentage, 0, 100) * 0.333f,
                };
                r.Add(buffForDamageDoneTemporal.valueId, buffForDamageDoneTemporal);

                if(emo != Emotion.pleasure)
                {
                    if(emo.IsGood())
                    {
                        if(inter.isValid)
                            favorabilityDefaultValue += inter.damagePercentage / 100f;
                        if(maxValueInter.isValid)
                            favorabilityDefaultValue += maxValueInter.times;
                    }
                    else
                    {
                        if(inter.isValid)
                            favorabilityDefaultValue -= inter.damagePercentage / 100f;
                        if(maxValueInter.isValid)
                            favorabilityDefaultValue -= maxValueInter.times;
                    }
                }

            }


            if(favorabilityDefaultValue != 0)
            {
                BuffForEmotionForCharacter buffForDefaultFavPermanent = new BuffForEmotionForCharacter()//Ex: the character make her feel bad/good, this wil be reflected in her favorability | PERMANENT
                {
                    characterID = from.ID,
                    emotion = Emotion.favorability,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = -1,
                    value = favorabilityDefaultValue,
                };
                r.Add(buffForDefaultFavPermanent.valueId, buffForDefaultFavPermanent);
            }
            return r;
        }


        public static Dictionary<(Emotion, EmotionModifier, Operation, int), BuffForEmotion> GenerateEmotionBuffBySceneInteractions(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted)
        {
            var r = new Dictionary<(Emotion, EmotionModifier, Operation, int), BuffForEmotion>();

            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);
            var emos = Enum.GetValues(typeof(Emotion));



            foreach(Emotion emo in emos)
            {
                if(emo == Emotion.All || emo == Emotion.None)
                    continue;

                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);

                if(inter.isValid)
                {
                    BuffForEmotion buffForDamageDonePermanent = new BuffForEmotion()//Ex: she get used to get the same type of damege overtime, instead of making her stronger it make her weaker | PERMANENT
                    {
                        emotion = emo,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = 1 + ((inter.damagePercentage / 100f) * (emo == Emotion.pleasure ? inter.damageScore * 2f : 1f)),
                    };
                    r.Add(buffForDamageDonePermanent.valueId, buffForDamageDonePermanent);
                }
            }



            foreach(Emotion emo in EmotionExt.emotionsWithDefaultValueBuff)
            {
                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);
                EmotionInteraction(archivedInteractions, emo, true, out Interaction maxValueInter);

                if(maxValueInter.isValid && maxValueInter.times > 0)
                {
                    BuffForEmotion buffByMaxValueReachedPermanent = new BuffForEmotion()//Ex: for when she is angry At EVERYONE couse max rage was reached | PERMANENT
                    {
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = emo == Emotion.pleasure ? 1f/*times already is greater that one*/ + ((float)(maxValueInter.times - 1) * 0.5f) : maxValueInter.times,
                    };
                    r.Add(buffByMaxValueReachedPermanent.valueId, buffByMaxValueReachedPermanent);

                    BuffForEmotion buffByMaxValueReachedTemporal = buffByMaxValueReachedPermanent;//Ex: for when she is angry At EVERYONE couse max rage was reached | TEMPORAL
                    buffByMaxValueReachedTemporal.value *= 5f;
                    buffByMaxValueReachedTemporal.durationInDays = 1;
                    r.Add(buffByMaxValueReachedTemporal.valueId, buffByMaxValueReachedTemporal);
                }
                else
                {
                    BuffForEmotion buffByMaxValueNotReachedPermanent = new BuffForEmotion()//Ex: if she was not angry to the max, then she has a change to calm down | PERMANENT
                    {
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = -(1f - Mathf.Clamp01(inter.damagePercentage / 100f)),
                    };
                    r.Add(buffByMaxValueNotReachedPermanent.valueId, buffByMaxValueNotReachedPermanent);
                }

                BuffForEmotion buffByDamageDoneTemporal = new BuffForEmotion()//Ex: for when she is angry at a character and she has not calm down | TEMPORAL
                {
                    emotion = emo,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = 1,
                    value = Mathf.Clamp(inter.damagePercentage, 0, 100) * 0.333f,
                };
                r.Add(buffByDamageDoneTemporal.valueId, buffByDamageDoneTemporal);
            }



            EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);

            if(orgasmInter.isValid && orgasmInter.times > 0)
            {
                BuffForEmotion buffByOrgasms = new BuffForEmotion()//Ex: she got horny couse many ogasm | TEMPORAL
                {
                    emotion = Emotion.arousal,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = 1,
                    value = orgasmInter.times * 3f,
                };
                r.Add(buffByOrgasms.valueId, buffByOrgasms);
            }



            return r;
        }
        static void EmotionInteraction(ICharactersSceneInteractions archivedInteractions, Emotion emo, bool maxValue, out Interaction interaction)
        {
            archivedInteractions.Peek(TriggeringBodyPart.All, SensitiveBodyPart.All, InterationReceivedType.All, emo, maxValue, out interaction);
        }

    }
}
