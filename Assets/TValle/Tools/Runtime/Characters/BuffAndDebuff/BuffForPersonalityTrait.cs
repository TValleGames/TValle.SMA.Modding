using Assets.TValle.Tools.Runtime.Characters.Atts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffForPersonalityTrait : IIdentifiableBuff<(PersonalityTraits, SimpleModifier, AddOperation, int)>, IStackableBuff<BuffForPersonalityTrait>, IEquatable<BuffForPersonalityTrait>
    {
        public PersonalityTraits trait;

        public SimpleModifier modifier;
        public AddOperation operation;
        public int durationInDays;
        public float value;



        public (PersonalityTraits, SimpleModifier, AddOperation, int) valueId => (trait, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();



        public bool IsStackableWith(ref BuffForPersonalityTrait Other)
        {
            return Other.trait == trait && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffForPersonalityTrait StackToNew(ref BuffForPersonalityTrait Other)
        {
            var r = this;
            switch(operation)
            {
                case AddOperation.None:
                    break;
                case AddOperation.subtract:
                case AddOperation.add:
                    r.value += Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffForPersonalityTrait Other)
        {
            switch(operation)
            {
                case AddOperation.None:
                    break;
                case AddOperation.subtract:
                case AddOperation.add:
                    value += Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }





        public override bool Equals(object obj) => this.Equals((BuffForPersonalityTrait)obj);
        public bool Equals(BuffForPersonalityTrait p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffForPersonalityTrait lhs, BuffForPersonalityTrait rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffForPersonalityTrait lhs, BuffForPersonalityTrait rhs) => !(lhs == rhs);
    }
}
