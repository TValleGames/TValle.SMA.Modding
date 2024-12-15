using Assets.TValle.Tools.Runtime.Characters.Atts;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff;
using Assets.TValle.Tools.Runtime.Characters.Intections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Scenes
{
    [Serializable]
    public class SceneCharacterFromToBuffAndDebuff
    {
        public SceneCharacter character;


        public Dictionary<(SimpleModifier, Operation, int), BuffOnKarma> BuffOnKarma;
        public Dictionary<(PersonalityTraits, SimpleModifier, Operation, int), BuffOnPersonalityTrait> BuffOnPersonalityTrait;
        public Dictionary<(Desires, EmotionModifier, Operation, int), BuffOnDesires> BuffOnDesires;
        public Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, SimpleModifier, Operation, int), BuffOnFavorabilityReqOfInteraction> BuffOnFavorabilityReqOfInteraction;
        public Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int), BuffOnInteraction> BuffOnInteraction;
        public Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura> BuffOnEmotionAura;
        public Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffOnEmotionTowardCharacter> BuffOnEmotionTowardCharacter;
        public Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion> BuffOnEmotion;


        public void DebugPrint()
        {
            DebugPrint(BuffOnKarma);
            DebugPrint(BuffOnPersonalityTrait);
            DebugPrint(BuffOnDesires);
            DebugPrint(BuffOnFavorabilityReqOfInteraction);
            DebugPrint(BuffOnInteraction);
            DebugPrint(BuffOnEmotionAura);
            DebugPrint(BuffOnEmotionTowardCharacter);
            DebugPrint(BuffOnEmotion);
        }
        static void DebugPrint<TKey, TValue>(Dictionary<TKey, TValue> dicc)
            where TValue : IPrintableBuff
        {
            if(dicc != null)
            {
                foreach(var item in dicc)
                {
                    Debug.LogWarning(item.Value.DebugPrint());
                }
            }

        }
    }
}
