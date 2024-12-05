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
    public struct BuffOnEmotion : IIdentifiableBuff<(Emotion, EmotionModifier, Operation, int)>, IStackableBuff<BuffOnEmotion>, IFloatValuableBuff
    {
        public Emotion emotion;

        public EmotionModifier modifier;
        public Operation operation;
        public int durationInDays;

        public float value;








        public (Emotion, EmotionModifier, Operation, int) valueId => (emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;

        public bool IsStackableWith(ref BuffOnEmotion Other)
        {
            return Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffOnEmotion StackToNew(ref BuffOnEmotion Other)
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

        public void StackToSelf(ref BuffOnEmotion Other)
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



        public override bool Equals(object obj) => this.Equals((BuffOnEmotion)obj);
        public bool Equals(BuffOnEmotion p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnEmotion lhs, BuffOnEmotion rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnEmotion lhs, BuffOnEmotion rhs) => !(lhs == rhs);
    }
}
