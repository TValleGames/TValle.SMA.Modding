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
        public SceneCharacterFromToBuffAndDebuff(SceneCharacter Character)
        {
            m_character = Character;
        }
        [SerializeField]
        [JustToReadUI]
        SceneCharacter m_character;

        public SceneCharacter character => m_character;

        public Dictionary<(SimpleModifier, Operation, int), BuffOnKarma> BuffOnKarma;
        public Dictionary<(PersonalityTraits, SimpleModifier, Operation, int), BuffOnPersonalityTrait> BuffOnPersonalityTrait;
        public Dictionary<(Desires, EmotionModifier, Operation, int), BuffOnDesires> BuffOnDesires;
        public Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, SimpleModifier, Operation, int), BuffOnFavorabilityReqOfInteraction> BuffOnFavorabilityReqOfInteraction;
        public Dictionary<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int), BuffOnInteraction> BuffOnInteraction;
        public Dictionary<(Emotion, SimpleEmotionModifier, Operation, int), BuffOnEmotionAura> BuffOnEmotionAura;
        public Dictionary<(Guid, Emotion, EmotionModifier, Operation, int), BuffOnEmotionTowardCharacter> BuffOnEmotionTowardCharacter;
        public Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion> BuffOnEmotion;

        public Dictionary<(SensitiveFemaleHoleWalls, SimpleModifier, AddOperation, int), BuffOnHoleWearingWalls> BuffOnHoleWearingWalls;
        public Dictionary<(SensitiveFemaleHoleBottom, SimpleModifier, AddOperation, int), BuffOnHoleWearingBottom> BuffOnHoleWearingBottom;
        public Dictionary<(SensitiveFemaleHole, SimpleModifier, AddOperation, int), BuffOnHoleWearingMotion> BuffOnHoleWearingMotion;

        public void Apply()
        {
            m_character?.GetComponentInChildren<IBuffableBySceneInteractionsCharacter>()?.Apply(this);
        }





        public void DebugPrint()
        {
            DebugPrint(m_character, BuffOnKarma);
            DebugPrint(m_character, BuffOnPersonalityTrait);
            DebugPrint(m_character, BuffOnDesires);
            DebugPrint(m_character, BuffOnFavorabilityReqOfInteraction);
            DebugPrint(m_character, BuffOnInteraction);
            DebugPrint(m_character, BuffOnEmotionAura);
            DebugPrint(m_character, BuffOnEmotionTowardCharacter);
            DebugPrint(m_character, BuffOnEmotion);

            DebugPrint(m_character, BuffOnHoleWearingWalls);
            DebugPrint(m_character, BuffOnHoleWearingBottom);
            DebugPrint(m_character, BuffOnHoleWearingMotion);
        }
        static void DebugPrint<TKey, TValue>(SceneCharacter character, Dictionary<TKey, TValue> dicc)
            where TValue : IPrintableBuff
        {
            if(dicc != null)
            {
                foreach(var item in dicc)
                {
                    Debug.LogWarning(character.fullName + " " + typeof(TValue).Name + " " + item.Value.DebugPrint());
                }
            }

        }
    }
}
