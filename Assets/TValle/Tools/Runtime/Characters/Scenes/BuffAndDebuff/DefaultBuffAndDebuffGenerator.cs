using Assets.TValle.Tools.Runtime.Characters;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff;
using Assets.TValle.Tools.Runtime.Characters.Intections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Scenes.BuffAndDebuff
{
    public static class DefaultBuffAndDebuffGenerator
    {


        public static Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int), BuffOnInteraction> GenerateBuffOnInteractionSceneInteractionsOnCharacterTo(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int), BuffOnInteraction>();
            target = to;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);


            var InterationReceivedTypes = Enum.GetValues(typeof(InterationReceivedType));
            var TriggeringBodyParts = Enum.GetValues(typeof(TriggeringBodyPart));
            var SensitiveBodyParts = Enum.GetValues(typeof(SensitiveBodyPart));

            foreach(InterationReceivedType interationReceivedType in InterationReceivedTypes)
            {
                foreach(TriggeringBodyPart triggeringBodyPart in TriggeringBodyParts)
                {
                    foreach(SensitiveBodyPart sensitiveBodyPart in SensitiveBodyParts)
                    {
                        archivedInteractions.Peek(triggeringBodyPart, sensitiveBodyPart, interationReceivedType, Emotion.pleasure, false, out Interaction pleasureInteraction);
                        archivedInteractions.Peek(triggeringBodyPart, sensitiveBodyPart, interationReceivedType, Emotion.pleasure, true, out Interaction orgasmInteraction);

                        if(pleasureInteraction.isValid || orgasmInteraction.isValid)
                        {
                            BuffOnInteraction buffForPainGainVsPleasureGivenPermanent = new BuffOnInteraction()//make harder to gain pain by this interaction
                            {
                                interationReceivedType = interationReceivedType,
                                fromPart = triggeringBodyPart,
                                toPart = sensitiveBodyPart,
                                emotion = Emotion.pain,
                                modifier = InteractionModifier.damage,
                                operation = ProductOperation.mult,
                                durationInDays = -1,
                                value = 999999999999999999/*TODO : mientras mas placer/orgasmos este estimulo genera, menos ganancia de dolor y de decepcin debe tener, rage se deja por fuera, por q ese va en buffFavReq*/,
                            };
                            r.Add(buffForPainGainVsPleasureGivenPermanent.valueId, buffForPainGainVsPleasureGivenPermanent);
                        }




                    }
                }
            }





            return r;
        }



        public static Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura> GenerateBuffOnEmotionAuraBySceneInteractionsOnCharacterFrom(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura>();
            target = from;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);

            #region the more he makes them feel good/bad the weaker/strong they get 
            {
                EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, true, out Interaction rageMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, true, out Interaction painMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, true, out Interaction disappointmentMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, true, out Interaction fearMaxInter);

                var positiveScoreByMaxValue = rageMaxInter.times - painMaxInter.times - disappointmentMaxInter.times - fearMaxInter.times + orgasmInter.times;
                var rageScoreByMaxValue = rageMaxInter.times - orgasmInter.times;
                var painScoreByMaxValue = painMaxInter.times - orgasmInter.times;
                var disappointmentScoreByMaxValue = disappointmentMaxInter.times - orgasmInter.times;
                var fearScoreByMaxValue = fearMaxInter.times - orgasmInter.times;

                if(positiveScoreByMaxValue != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to pleasure damage  | PERMANENT
                    {
                        emotion = Emotion.pleasure,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (positiveScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(rageScoreByMaxValue != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to rage damage  | PERMANENT
                    {
                        emotion = Emotion.rage,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (rageScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(painScoreByMaxValue != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to pain damage  | PERMANENT
                    {
                        emotion = Emotion.pain,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (painScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(disappointmentScoreByMaxValue != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to disappointment damage  | PERMANENT
                    {
                        emotion = Emotion.disappointment,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (disappointmentScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(fearScoreByMaxValue != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to fear damage  | PERMANENT
                    {
                        emotion = Emotion.fear,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (fearScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }

                EmotionInteraction(archivedInteractions, Emotion.pleasure, false, out Interaction pleasureInter);
                EmotionInteraction(archivedInteractions, Emotion.happiness, false, out Interaction happinessInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, false, out Interaction rageInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, false, out Interaction painInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, false, out Interaction disappointmentInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, false, out Interaction fearInter);

                var pleasureDamage = pleasureInter.damagePercentage * (pleasureInter.damageScore * 2);

                var positiveDamage = pleasureDamage + happinessInter.damagePercentage;
                var negativeDamage = rageInter.damagePercentage + painInter.damagePercentage + disappointmentInter.damagePercentage + fearInter.damagePercentage;

                var pleasureScore = pleasureDamage - negativeDamage;
                var happinessScore = happinessInter.damagePercentage - negativeDamage;
                var rageScore = rageInter.damagePercentage - positiveDamage;
                var painScore = painInter.damagePercentage - positiveDamage;
                var disappointmentScore = disappointmentInter.damagePercentage - positiveDamage;
                var fearScore = fearInter.damagePercentage - positiveDamage;

                if(pleasureScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to pleasure damage  | PERMANENT
                    {
                        emotion = Emotion.pleasure,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (pleasureScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(happinessScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to pleasure damage  | PERMANENT
                    {
                        emotion = Emotion.happiness,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (happinessScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(rageScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to rage damage  | PERMANENT
                    {
                        emotion = Emotion.rage,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (rageScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(painScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to pain damage  | PERMANENT
                    {
                        emotion = Emotion.pain,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (painScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(disappointmentScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to disappointment damage  | PERMANENT
                    {
                        emotion = Emotion.disappointment,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (disappointmentScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(fearScore != 0)
                {
                    BuffOnEmotionAura buffForMaxValuesPermanent = new BuffOnEmotionAura()//Ex: he makes everyone around weaker/stronger to fear damage  | PERMANENT
                    {
                        emotion = Emotion.fear,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (fearScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
            }
            #endregion

            return r;
        }
        public static Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura> GenerateBuffOnEmotionAuraBySceneInteractionsOnCharacterTo(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura>();
            target = to;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);


            #region the more she makes cum men, the more she makes men exited by default 
            archivedInteractions.Peek(TriggeringBodyPart.semen, SensitiveBodyPart.All, InterationReceivedType.pouringIn, Emotion.pleasure, false, out Interaction semenPouringInInteraction);
            archivedInteractions.Peek(TriggeringBodyPart.semen, SensitiveBodyPart.All, InterationReceivedType.pouringOn, Emotion.pleasure, false, out Interaction semenPouringOnInteraction);
            if(semenPouringOnInteraction.isValid)
            {
                BuffOnEmotionAura buffForTimesPermanentPouringOnPermanent = new BuffOnEmotionAura()//Ex: she turns every one exited by defualt | PERMANENT
                {
                    emotion = Emotion.pleasure,
                    modifier = SimpleEmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = -1,
                    value = (float)semenPouringOnInteraction.times / 40f,
                };
                r.Add(buffForTimesPermanentPouringOnPermanent.valueId, buffForTimesPermanentPouringOnPermanent);
            }
            if(semenPouringInInteraction.isValid)
            {
                BuffOnEmotionAura buffForTimesPermanentPouringInPermanent = new BuffOnEmotionAura()//Ex: she turns every one exited by defualt | PERMANENT
                {
                    emotion = Emotion.pleasure,
                    modifier = SimpleEmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = -1,
                    value = (float)semenPouringInInteraction.times / 40f,
                };
                r.Add(buffForTimesPermanentPouringInPermanent.valueId, buffForTimesPermanentPouringInPermanent);
            }
            #endregion 

            #region the more positive/negative stimuli she turns every men more resistant or weaker to disgust
            {
                EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, true, out Interaction rageMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, true, out Interaction painMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, true, out Interaction disappointmentMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, true, out Interaction fearMaxInter);


                var disgustByMaxValueResult = rageMaxInter.times + painMaxInter.times + disappointmentMaxInter.times + fearMaxInter.times - orgasmInter.times;

                if(disgustByMaxValueResult != 0)
                {
                    BuffOnEmotionAura buffForPositveAndNegativeMaxValuesPermanent = new BuffOnEmotionAura()//Ex: she turns every one more resistant or weaker defualt | PERMANENT
                    {
                        emotion = Emotion.disgust,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + ((disgustByMaxValueResult < 0 ? 1f / -disgustByMaxValueResult : 1 * disgustByMaxValueResult) * 0.01f), 0, 2),
                    };
                    r.Add(buffForPositveAndNegativeMaxValuesPermanent.valueId, buffForPositveAndNegativeMaxValuesPermanent);
                }


                EmotionInteraction(archivedInteractions, Emotion.pleasure, false, out Interaction pleasureInter);
                EmotionInteraction(archivedInteractions, Emotion.happiness, false, out Interaction happinessInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, false, out Interaction rageInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, false, out Interaction painInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, false, out Interaction disappointmentInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, false, out Interaction fearInter);

                var disgustByDamagePercentageResult = rageInter.damagePercentage + painInter.damagePercentage + disappointmentInter.damagePercentage + fearInter.damagePercentage - (pleasureInter.damagePercentage * pleasureInter.damageScore * 2) - happinessInter.damagePercentage;
                disgustByDamagePercentageResult /= 100f;

                if(disgustByDamagePercentageResult != 0)
                {
                    BuffOnEmotionAura buffForPositveAndNegativeDamageDonePermanent = new BuffOnEmotionAura()//Ex: she turns every one more resistant or weaker defualt | PERMANENT
                    {
                        emotion = Emotion.disgust,
                        modifier = SimpleEmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + ((disgustByMaxValueResult < 0 ? 1f / -disgustByMaxValueResult : 1 * disgustByMaxValueResult) * 0.01f), 0, 2),
                    };
                    r.Add(buffForPositveAndNegativeDamageDonePermanent.valueId, buffForPositveAndNegativeDamageDonePermanent);
                }
            }
            #endregion

            return r;
        }





        public static Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffOnEmotionTowardCharacter> GenerateBuffOnEmotionTowardCharacterBySceneInteractionsOnCharacterTo(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffOnEmotionTowardCharacter>();
            target = to;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);

            var favorabilityDefaultValue = 0f;



            foreach(Emotion emo in EmotionExt.femaleEmotionsWithDefaultValueBuff)
            {
                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);
                EmotionInteraction(archivedInteractions, emo, true, out Interaction maxValueInter);

                #region constant hate/love etc from a single character, permanent and temporal for a day
                if(maxValueInter.isValid && maxValueInter.times > 0)
                {
                    BuffOnEmotionTowardCharacter buffByMaxValueReachedPermanent = new BuffOnEmotionTowardCharacter()//Ex: for when she is angry at a character couse max rage was reached | PERMANENT
                    {
                        towardID = from.ID,
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = emo == Emotion.pleasure ? (float)maxValueInter.times * 0.2f : maxValueInter.times,
                    };
                    r.Add(buffByMaxValueReachedPermanent.valueId, buffByMaxValueReachedPermanent);

                    BuffOnEmotionTowardCharacter buffByMaxValueReachedTemporal = buffByMaxValueReachedPermanent;//Ex: for when she is angry at a character couse max rage was reached | TEMPORAL
                    buffByMaxValueReachedTemporal.value *= 5f;
                    buffByMaxValueReachedTemporal.durationInDays = 1;
                    r.Add(buffByMaxValueReachedTemporal.valueId, buffByMaxValueReachedTemporal);
                }
                else
                {
                    if(!sceneAborted)
                    {
                        BuffOnEmotionTowardCharacter buffByMaxValueNotReachedPermanent = new BuffOnEmotionTowardCharacter()//Ex: if she was not angry to the max, then she has a change to calm down | PERMANENT
                        {
                            towardID = from.ID,
                            emotion = emo,
                            modifier = EmotionModifier.defaultValue,
                            operation = Operation.add,
                            durationInDays = -1,
                            value = -(1f - Mathf.Clamp01(inter.damagePercentage / 100f)),
                        };
                        r.Add(buffByMaxValueNotReachedPermanent.valueId, buffByMaxValueNotReachedPermanent);
                    }
                }

                BuffOnEmotionTowardCharacter buffForDamageDoneTemporal = new BuffOnEmotionTowardCharacter()//Ex: for when she is angry at a character and she has not calm down | TEMPORAL
                {
                    towardID = from.ID,
                    emotion = emo,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = 1,
                    value = Mathf.Clamp(inter.damagePercentage, 0, 100) * 0.333f,
                };
                AddOrStack(r, ref buffForDamageDoneTemporal);
                #endregion

                #region favorability gain by good/bad changes in emotions
                if(emo.IsGood())
                {
                    if(inter.isValid)
                        favorabilityDefaultValue += inter.damagePercentage / 100f * (emo == Emotion.pleasure ? inter.damageScore : 1f) * 0.2f;
                    if(maxValueInter.isValid)
                        favorabilityDefaultValue += maxValueInter.times * 0.2f;
                }
                else
                {
                    if(inter.isValid)
                        favorabilityDefaultValue -= inter.damagePercentage / 100f * 0.5f;
                    if(maxValueInter.isValid)
                        favorabilityDefaultValue -= maxValueInter.times * 0.5f;
                }
                #endregion

            }

            #region favorability gain by good/bad changes in emotions
            if(favorabilityDefaultValue != 0)
            {
                BuffOnEmotionTowardCharacter buffForDefaultFavPermanent = new BuffOnEmotionTowardCharacter()//Ex: the character make her feel bad/good, this wil be reflected in her favorability | PERMANENT
                {
                    towardID = from.ID,
                    emotion = Emotion.favorability,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = -1,
                    value = favorabilityDefaultValue,
                };
                r.Add(buffForDefaultFavPermanent.valueId, buffForDefaultFavPermanent);
            }
            #endregion
            return r;
        }




        public static Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion> GenerateBuffOnEmotionBySceneInteractionsOnCharacterTo(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion>();
            target = to;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);
            var emos = Enum.GetValues(typeof(Emotion));


            #region making weaker and waker to emotion damage
            foreach(Emotion emo in emos)
            {
                if(emo == Emotion.All || emo == Emotion.None)
                    continue;

                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);

                if(inter.isValid)
                {
                    BuffOnEmotion buffForDamageDonePermanent = new BuffOnEmotion()//Ex: she get used to get the same type of damege overtime, instead of making her stronger it make her weaker | PERMANENT
                    {
                        emotion = emo,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = 1 + (0.01f * (inter.damagePercentage / 100f) * (emo == Emotion.pleasure ? inter.damageScore * 2f : 1f)),
                    };
                    r.Add(buffForDamageDonePermanent.valueId, buffForDamageDonePermanent);
                }
            }
            #endregion


            #region make her stronger or weaker, based on positive or negative stimuli
            {
                EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, true, out Interaction rageMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, true, out Interaction painMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, true, out Interaction disappointmentMaxInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, true, out Interaction fearMaxInter);

                var positiveScoreByMaxValue = rageMaxInter.times - painMaxInter.times - disappointmentMaxInter.times - fearMaxInter.times + orgasmInter.times;
                var rageScoreByMaxValue = rageMaxInter.times - orgasmInter.times;
                var painScoreByMaxValue = painMaxInter.times - orgasmInter.times;
                var disappointmentScoreByMaxValue = disappointmentMaxInter.times - orgasmInter.times;
                var fearScoreByMaxValue = fearMaxInter.times - orgasmInter.times;

                if(positiveScoreByMaxValue != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to pleasure damage  | PERMANENT
                    {
                        emotion = Emotion.pleasure,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (positiveScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(rageScoreByMaxValue != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to rage damage  | PERMANENT
                    {
                        emotion = Emotion.rage,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (rageScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(painScoreByMaxValue != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to pain damage  | PERMANENT
                    {
                        emotion = Emotion.pain,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (painScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(disappointmentScoreByMaxValue != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to disappointment damage  | PERMANENT
                    {
                        emotion = Emotion.disappointment,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (disappointmentScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }
                if(fearScoreByMaxValue != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to fear damage  | PERMANENT
                    {
                        emotion = Emotion.fear,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (fearScoreByMaxValue * 0.01f), 0, 2),
                    };
                    r.Add(buffForMaxValuesPermanent.valueId, buffForMaxValuesPermanent);
                }

                EmotionInteraction(archivedInteractions, Emotion.pleasure, false, out Interaction pleasureInter);
                EmotionInteraction(archivedInteractions, Emotion.happiness, false, out Interaction happinessInter);
                EmotionInteraction(archivedInteractions, Emotion.rage, false, out Interaction rageInter);
                EmotionInteraction(archivedInteractions, Emotion.pain, false, out Interaction painInter);
                EmotionInteraction(archivedInteractions, Emotion.disappointment, false, out Interaction disappointmentInter);
                EmotionInteraction(archivedInteractions, Emotion.fear, false, out Interaction fearInter);

                var pleasureDamage = pleasureInter.damagePercentage * (pleasureInter.damageScore * 2);

                var positiveDamage = pleasureDamage + happinessInter.damagePercentage;
                var negativeDamage = rageInter.damagePercentage + painInter.damagePercentage + disappointmentInter.damagePercentage + fearInter.damagePercentage;

                var pleasureScore = pleasureDamage - negativeDamage;
                var happinessScore = happinessInter.damagePercentage - negativeDamage;
                var rageScore = rageInter.damagePercentage - positiveDamage;
                var painScore = painInter.damagePercentage - positiveDamage;
                var disappointmentScore = disappointmentInter.damagePercentage - positiveDamage;
                var fearScore = fearInter.damagePercentage - positiveDamage;

                if(pleasureScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to pleasure damage  | PERMANENT
                    {
                        emotion = Emotion.pleasure,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (pleasureScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(happinessScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to happiness damage  | PERMANENT
                    {
                        emotion = Emotion.happiness,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (happinessScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(rageScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to rage damage  | PERMANENT
                    {
                        emotion = Emotion.rage,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (rageScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(painScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to pain damage  | PERMANENT
                    {
                        emotion = Emotion.pain,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (painScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(disappointmentScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to disappointment damage  | PERMANENT
                    {
                        emotion = Emotion.disappointment,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (disappointmentScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
                if(fearScore != 0)
                {
                    BuffOnEmotion buffForMaxValuesPermanent = new BuffOnEmotion()//Ex: makes her weaker/stronger to fear damage  | PERMANENT
                    {
                        emotion = Emotion.fear,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1 + (fearScore * 0.01f * 0.01f), 0, 2),
                    };
                    AddOrStack(r, ref buffForMaxValuesPermanent);
                }
            }
            #endregion


            #region makes her have increase/ reduce residual emotion values, perpanent and along a single day
            foreach(Emotion emo in EmotionExt.femaleEmotionsWithDefaultValueBuff)
            {
                EmotionInteraction(archivedInteractions, emo, false, out Interaction inter);
                EmotionInteraction(archivedInteractions, emo, true, out Interaction maxValueInter);

                if(maxValueInter.isValid && maxValueInter.times > 0)
                {
                    BuffOnEmotion buffByMaxValueReachedPermanent = new BuffOnEmotion()//Ex: for when she is angry At EVERYONE couse max rage was reached | PERMANENT
                    {
                        emotion = emo,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = emo == Emotion.pleasure ? (float)maxValueInter.times * 0.2f : maxValueInter.times,
                    };
                    r.Add(buffByMaxValueReachedPermanent.valueId, buffByMaxValueReachedPermanent);

                    BuffOnEmotion buffByMaxValueReachedTemporal = buffByMaxValueReachedPermanent;//Ex: for when she is angry At EVERYONE couse max rage was reached | TEMPORAL
                    buffByMaxValueReachedTemporal.value *= 5f;
                    buffByMaxValueReachedTemporal.durationInDays = 1;
                    r.Add(buffByMaxValueReachedTemporal.valueId, buffByMaxValueReachedTemporal);
                }
                else
                {
                    if(!sceneAborted)
                    {
                        BuffOnEmotion buffByMaxValueNotReachedPermanent = new BuffOnEmotion()//Ex: if she was not angry to the max, then she has a change to calm down | PERMANENT
                        {
                            emotion = emo,
                            modifier = EmotionModifier.defaultValue,
                            operation = Operation.add,
                            durationInDays = -1,
                            value = -(1f - Mathf.Clamp01(inter.damagePercentage / 100f)),
                        };
                        r.Add(buffByMaxValueNotReachedPermanent.valueId, buffByMaxValueNotReachedPermanent);
                    }
                }

                BuffOnEmotion buffByDamageDoneTemporal = new BuffOnEmotion()//Ex: for when she is angry at a character and she has not calm down | TEMPORAL
                {
                    emotion = emo,
                    modifier = EmotionModifier.defaultValue,
                    operation = Operation.add,
                    durationInDays = 1,
                    value = Mathf.Clamp(inter.damagePercentage, 0, 100) * 0.333f,
                };
                AddOrStack(r, ref buffByDamageDoneTemporal);
            }
            #endregion

            #region makes her horny based in orgasm count in the scene , permanent and temporal for a day
            {
                EmotionInteraction(archivedInteractions, Emotion.pleasure, true, out Interaction orgasmInter);

                if(orgasmInter.isValid)
                {
                    BuffOnEmotion buffByOrgasmsPermanent = new BuffOnEmotion()//Ex: she got horny couse many ogasm | PERMANENT
                    {
                        emotion = Emotion.arousal,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = -1,
                        value = orgasmInter.times * 0.1f,
                    };
                    BuffOnEmotion buffByOrgasms = new BuffOnEmotion()//Ex: she got horny couse many ogasm | TEMPORAL
                    {
                        emotion = Emotion.arousal,
                        modifier = EmotionModifier.defaultValue,
                        operation = Operation.add,
                        durationInDays = 1,
                        value = orgasmInter.times * 2f,
                    };
                    r.Add(buffByOrgasmsPermanent.valueId, buffByOrgasmsPermanent);
                    r.Add(buffByOrgasms.valueId, buffByOrgasms);
                }
            }
            #endregion


            return r;
        }
        public static Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion> GenerateBuffOnEmotionBySceneInteractionsOnCharacterFrom(ISceneInteractions sceneInteractions, SceneCharacter from, SceneCharacter to, bool sceneAborted, out SceneCharacter target)
        {
            var r = new Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion>();
            target = from;
            var archivedInteractions = sceneInteractions.GetArchivedInteractions(from, to);

            #region resistance to disgust on male based on male orgasm
            {
                archivedInteractions.Peek(TriggeringBodyPart.semen, SensitiveBodyPart.All, InterationReceivedType.pouringIn, Emotion.pleasure, false, out Interaction semenPouringInInteraction);
                archivedInteractions.Peek(TriggeringBodyPart.semen, SensitiveBodyPart.All, InterationReceivedType.pouringOn, Emotion.pleasure, false, out Interaction semenPouringOnInteraction);

                var eyaculatedTimes = semenPouringInInteraction.times + semenPouringOnInteraction.times;

                if(eyaculatedTimes > 0)
                {
                    BuffOnEmotion buffForEyaculationsPermanent = new BuffOnEmotion()//Ex: the more he eyaculates the more resistant he turns | PERMANENT
                    {
                        emotion = Emotion.disgust,
                        modifier = EmotionModifier.gain,
                        operation = Operation.mult,
                        durationInDays = -1,
                        value = Mathf.Clamp(1f - ((float)eyaculatedTimes * 0.01f * 0.1f), 0, 2),
                    };
                    r.Add(buffForEyaculationsPermanent.valueId, buffForEyaculationsPermanent);
                }
            }
            #endregion


            #region resistance to pleasure on male based on pleasure given with his penis
            archivedInteractions.Peek(TriggeringBodyPart.penis, SensitiveBodyPart.All, InterationReceivedType.All, Emotion.pleasure, false, out Interaction pleasureByDickInteraction);
            if(pleasureByDickInteraction.isValid)
            {
                BuffOnEmotion buffForPleasureByDickPermanent = new BuffOnEmotion()//Ex: the more he gives pleaure with dick the more resistant he turns | PERMANENT
                {
                    emotion = Emotion.pleasure,
                    modifier = EmotionModifier.gain,
                    operation = Operation.mult,
                    durationInDays = -1,
                    value = Mathf.Clamp(1f - ((pleasureByDickInteraction.damagePercentage / 100 * (pleasureByDickInteraction.damageScore * 2)) * 0.01f), 0, 2),
                };
                r.Add(buffForPleasureByDickPermanent.valueId, buffForPleasureByDickPermanent);
            }
            #endregion

            return r;
        }





        static void EmotionInteraction(ICharactersSceneInteractions archivedInteractions, Emotion emo, bool maxValue, out Interaction interaction)
        {
            archivedInteractions.Peek(TriggeringBodyPart.All, SensitiveBodyPart.All, InterationReceivedType.All, emo, maxValue, out interaction);
        }
        static void AddOrStack<TKey, TBuff>(Dictionary<TKey, TBuff> dicc, ref TBuff other)
           where TKey : struct, ITuple
           where TBuff : IIdentifiableBuff<TKey>, IFloatValuableBuff, IStackableBuff<TBuff>
        {
            if(other.buffValue == 0)
                return;
            if(dicc.TryGetValue(other.valueId, out TBuff existente))
            {
                existente.StackToSelf(ref other);
                dicc[other.valueId] = existente;
            }
            else
            {
                dicc.Add(other.valueId, other);
            }
        }

    }
}
