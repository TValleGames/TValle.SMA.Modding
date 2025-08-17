using Assets.TValle.Tools.Runtime.Characters.Atts;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff;
using Assets.TValle.Tools.Runtime.Characters.Intections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public Dictionary<ITuple, BuffOnKarma> BuffOnKarma;
        public Dictionary<ITuple, BuffOnPersonalityTrait> BuffOnPersonalityTrait;
        public Dictionary<ITuple, BuffOnDesires> BuffOnDesires;
        public Dictionary<ITuple, BuffOnFavorabilityReqOfInteraction> BuffOnFavorabilityReqOfInteraction;
        public Dictionary<ITuple, BuffOnInteraction> BuffOnInteraction;
        public Dictionary<ITuple, BuffOnEmotionAura> BuffOnEmotionAura;
        public Dictionary<ITuple, BuffOnEmotionTowardCharacter> BuffOnEmotionTowardCharacter;
        public Dictionary<ITuple, BuffOnEmotion> BuffOnEmotion;

        public Dictionary<ITuple, BuffOnHoleWearingWalls> BuffOnHoleWearingWalls;
        public Dictionary<ITuple, BuffOnHoleWearingBottom> BuffOnHoleWearingBottom;
        public Dictionary<ITuple, BuffOnHoleWearingMotion> BuffOnHoleWearingMotion;

        public Dictionary<ITuple, BuffOnOxygenDemand> BuffOnOxygenDemand;


        public void Apply()
        {
            m_character?.GetComponentInChildren<IBuffableBySceneInteractionsCharacter>()?.Apply(this);
        }


        public List<IPrintableBuff> GetAllPrintables()
        {

            var rr = GetPrintables(BuffOnKarma)
                .Concat(GetPrintables(BuffOnPersonalityTrait))
                 .Concat(GetPrintables(BuffOnDesires))
                  .Concat(GetPrintables(BuffOnFavorabilityReqOfInteraction))
                   .Concat(GetPrintables(BuffOnInteraction))
                    .Concat(GetPrintables(BuffOnEmotionAura))
                     .Concat(GetPrintables(BuffOnEmotionTowardCharacter))
                      .Concat(GetPrintables(BuffOnEmotion))
                       .Concat(GetPrintables(BuffOnHoleWearingWalls))
                        .Concat(GetPrintables(BuffOnHoleWearingBottom))
                         .Concat(GetPrintables(BuffOnHoleWearingMotion))
                          .Concat(GetPrintables(BuffOnOxygenDemand))
                          .ToList();
            return rr;
        }

        static IEnumerable<IPrintableBuff> GetPrintables<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dicc)
            where TValue : IPrintableBuff, IValidableBuff, IFloatValuableBuff
        {
            return (dicc?.Values ?? Array.Empty<TValue>()).Where(buff => buff.isValid && buff.buffValue != 0).Cast<IPrintableBuff>();
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
            
            DebugPrint(m_character, BuffOnOxygenDemand);

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
