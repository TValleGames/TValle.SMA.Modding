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
    public struct BuffOnEmotionAura : IIdentifiableBuff<(Emotion, SimpleEmotionModifier, Operation, int)>, IStackableBuff<BuffOnEmotionAura>, IFloatValuableBuff, IEndableOnDateBuff
    {
        public Emotion emotion;

        public SimpleEmotionModifier modifier;
        public Operation operation;
        public int endHour;

        public float value;







        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (Emotion, SimpleEmotionModifier, Operation, int) valueId => (emotion, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();

        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnEmotionAura Other)
        {
            return Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnEmotionAura StackToNew(ref BuffOnEmotionAura Other)
        {
            var r = this;
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.add:
                    r.value += Other.value;
                    break;
                case Operation.mult:
                    r.value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffOnEmotionAura Other)
        {
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.add:
                    value += Other.value;
                    break;
                case Operation.mult:
                    value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }



        public override bool Equals(object obj) => this.Equals((BuffOnEmotionAura)obj);
        public bool Equals(BuffOnEmotionAura p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnEmotionAura lhs, BuffOnEmotionAura rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnEmotionAura lhs, BuffOnEmotionAura rhs) => !(lhs == rhs);
    }
}
