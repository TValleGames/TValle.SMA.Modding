using Assets.TValle.Tools.Runtime.Characters.Atts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffOnPersonalityTrait : IIdentifiableBuff<(PersonalityTraits, SimpleModifier, AddOperation, int)>, IStackableBuff<BuffOnPersonalityTrait>, IEquatable<BuffOnPersonalityTrait>, IFloatValuableBuff
    {
        public PersonalityTraits trait;

        public SimpleModifier modifier;
        public AddOperation operation;
        public int durationInDays;
        public float value;



        public (PersonalityTraits, SimpleModifier, AddOperation, int) valueId => (trait, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnPersonalityTrait Other)
        {
            return Other.trait == trait && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffOnPersonalityTrait StackToNew(ref BuffOnPersonalityTrait Other)
        {
            var r = this;
            switch(operation)
            {
                case AddOperation.None:
                    break;
                case AddOperation.add:
                    r.value += Other.value;
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
                case AddOperation.None:
                    break;
                case AddOperation.add:
                    value += Other.value;
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
