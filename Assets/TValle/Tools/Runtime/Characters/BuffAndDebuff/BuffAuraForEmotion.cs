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
    public struct BuffAuraForEmotion : IIdentifiableBuff<(Emotion, SimpleModifier, Operation, int)>, IStackableBuff<BuffAuraForEmotion>
    {
        public Emotion emotion;

        public SimpleModifier modifier;
        public Operation operation;
        public int durationInDays;

        public float value;








        public (Emotion, SimpleModifier, Operation, int) valueId => (emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();




        public bool IsStackableWith(ref BuffAuraForEmotion Other)
        {
            return Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffAuraForEmotion StackToNew(ref BuffAuraForEmotion Other)
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

        public void StackToSelf(ref BuffAuraForEmotion Other)
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



        public override bool Equals(object obj) => this.Equals((BuffAuraForEmotion)obj);
        public bool Equals(BuffAuraForEmotion p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffAuraForEmotion lhs, BuffAuraForEmotion rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffAuraForEmotion lhs, BuffAuraForEmotion rhs) => !(lhs == rhs);
    }
}
