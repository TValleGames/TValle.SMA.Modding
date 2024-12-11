using Assets.TValle.Tools.Runtime.Characters.Atts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffOnPersonalityTrait : IIdentifiableBuff<(PersonalityTraits, SimpleModifier, Operation, int)>, IStackableBuff<BuffOnPersonalityTrait>, IEquatable<BuffOnPersonalityTrait>, IFloatValuableBuff, IEndableOnDateBuff
    {
        public PersonalityTraits trait;

        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;


        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (PersonalityTraits, SimpleModifier, Operation, int) valueId => (trait, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnPersonalityTrait Other)
        {
            return Other.trait == trait && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnPersonalityTrait StackToNew(ref BuffOnPersonalityTrait Other)
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

        public void StackToSelf(ref BuffOnPersonalityTrait Other)
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





        public override bool Equals(object obj) => this.Equals((BuffOnPersonalityTrait)obj);
        public bool Equals(BuffOnPersonalityTrait p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnPersonalityTrait lhs, BuffOnPersonalityTrait rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnPersonalityTrait lhs, BuffOnPersonalityTrait rhs) => !(lhs == rhs);
    }
}
