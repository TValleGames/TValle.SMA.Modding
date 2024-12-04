using Assets.TValle.Tools.Runtime.Characters.Atts;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    [Serializable]
    public struct BuffForEmotionForCharacter : IIdentifiableBuff<(Guid,Emotion, EmotionModifier, Operation,  int)>, IStackableBuff<BuffForEmotionForCharacter>
    {
        public Guid characterID;
        public Emotion emotion;

        public EmotionModifier modifier;
        public Operation operation;
        public int durationInDays;
        public float value;








        public (Guid, Emotion, EmotionModifier, Operation, int) valueId => (characterID,emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();




        public bool IsStackableWith(ref BuffForEmotionForCharacter Other)
        {
            return Other.characterID == characterID && Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffForEmotionForCharacter StackToNew(ref BuffForEmotionForCharacter Other)
        {
            var r = this;
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.subtract:
                case Operation.add:
                    r.value += Other.value;
                    break;
                case Operation.mult:
                case Operation.divide:
                    r.value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffForEmotionForCharacter Other)
        {
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.subtract:
                case Operation.add:
                    value += Other.value;
                    break;
                case Operation.mult:
                case Operation.divide:
                    value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }



        public override bool Equals(object obj) => this.Equals((BuffForEmotionForCharacter)obj);
        public bool Equals(BuffForEmotionForCharacter p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffForEmotionForCharacter lhs, BuffForEmotionForCharacter rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffForEmotionForCharacter lhs, BuffForEmotionForCharacter rhs) => !(lhs == rhs);
    }
}
