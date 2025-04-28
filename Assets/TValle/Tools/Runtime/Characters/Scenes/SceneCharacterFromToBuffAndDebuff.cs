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

            if(Character == null)
                throw new ArgumentNullException("Character", "Character null reference.");
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
        public Dictionary<(string, Emotion, EmotionModifier, Operation, int), BuffOnEmotionTowardCharacter> BuffOnEmotionTowardCharacter;
        public Dictionary<(Emotion, EmotionModifier, Operation, int), BuffOnEmotion> BuffOnEmotion;

        public Dictionary<(SensitiveFemaleHoleWalls, SimpleModifier, AddOperation, int), BuffOnHoleWearingWalls> BuffOnHoleWearingWalls;
        public Dictionary<(SensitiveFemaleHoleBottom, SimpleModifier, AddOperation, int), BuffOnHoleWearingBottom> BuffOnHoleWearingBottom;
        public Dictionary<(SensitiveFemaleHole, SimpleModifier, AddOperation, int), BuffOnHoleWearingMotion> BuffOnHoleWearingMotion;

        public void Apply()
        {
            m_character?.GetComponentInChildren<IBuffableBySceneInteractionsCharacter>()?.Apply(this);
        }


        public List<IPrintableBuff> GetAllPrintables()
        {
            var r = (BuffOnKarma?.Values ?? (IEnumerable<BuffOnKarma>)Array.Empty<BuffOnKarma>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>()
                .Concat((BuffOnPersonalityTrait?.Values ?? (IEnumerable<BuffOnPersonalityTrait>)Array.Empty<BuffOnPersonalityTrait>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                 .Concat((BuffOnDesires?.Values ?? (IEnumerable<BuffOnDesires>)Array.Empty<BuffOnDesires>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                  .Concat((BuffOnFavorabilityReqOfInteraction?.Values ?? (IEnumerable<BuffOnFavorabilityReqOfInteraction>)Array.Empty<BuffOnFavorabilityReqOfInteraction>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                   .Concat((BuffOnInteraction?.Values ?? (IEnumerable<BuffOnInteraction>)Array.Empty<BuffOnInteraction>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                    .Concat((BuffOnEmotionAura?.Values ?? (IEnumerable<BuffOnEmotionAura>)Array.Empty<BuffOnEmotionAura>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                     .Concat((BuffOnEmotionTowardCharacter?.Values ?? (IEnumerable<BuffOnEmotionTowardCharacter>)Array.Empty<BuffOnEmotionTowardCharacter>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                      .Concat((BuffOnEmotion?.Values ?? (IEnumerable<BuffOnEmotion>)Array.Empty<BuffOnEmotion>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                       .Concat((BuffOnHoleWearingWalls?.Values ?? (IEnumerable<BuffOnHoleWearingWalls>)Array.Empty<BuffOnHoleWearingWalls>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                        .Concat((BuffOnHoleWearingBottom?.Values ?? (IEnumerable<BuffOnHoleWearingBottom>)Array.Empty<BuffOnHoleWearingBottom>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                         .Concat((BuffOnHoleWearingMotion?.Values ?? (IEnumerable<BuffOnHoleWearingMotion>)Array.Empty<BuffOnHoleWearingMotion>()).Where(buff => buff.isValid && buff.value != 0).Cast<IPrintableBuff>())
                         .ToList();
            return r;
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
