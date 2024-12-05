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
    public struct BuffOnEmotionTowardCharacter : IIdentifiableBuff<(Guid,Emotion, EmotionModifier, Operation,  int)>, IStackableBuff<BuffOnEmotionTowardCharacter>, IFloatValuableBuff
    {
        public Guid towardID;
        public Emotion emotion;

        public EmotionModifier modifier;
        public Operation operation;
        public int durationInDays;
        public float value;








        public (Guid, Emotion, EmotionModifier, Operation, int) valueId => (towardID,emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;



        public bool IsStackableWith(ref BuffOnEmotionTowardCharacter Other)
        {
            return Other.towardID == towardID && Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffOnEmotionTowardCharacter StackToNew(ref BuffOnEmotionTowardCharacter Other)
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

        public void StackToSelf(ref BuffOnEmotionTowardCharacter Other)
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



        public override bool Equals(object obj) => this.Equals((BuffOnEmotionTowardCharacter)obj);
        public bool Equals(BuffOnEmotionTowardCharacter p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnEmotionTowardCharacter lhs, BuffOnEmotionTowardCharacter rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnEmotionTowardCharacter lhs, BuffOnEmotionTowardCharacter rhs) => !(lhs == rhs);
    }
}
