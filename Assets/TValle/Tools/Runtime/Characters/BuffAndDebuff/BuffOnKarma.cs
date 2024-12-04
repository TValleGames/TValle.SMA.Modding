using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffOnKarma : IIdentifiableBuff<(SimpleModifier, Operation, int)>, IStackableBuff<BuffOnKarma>
    {
        public SimpleModifier modifier;
        public Operation operation;
        public int durationInDays;
        public float value;

        public (SimpleModifier, Operation, int) valueId => (modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();




        public bool IsStackableWith(ref BuffOnKarma Other)
        {
            return Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffOnKarma StackToNew(ref BuffOnKarma Other)
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

        public void StackToSelf(ref BuffOnKarma Other)
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



        public override bool Equals(object obj) => this.Equals((BuffOnKarma)obj);
        public bool Equals(BuffOnKarma p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnKarma lhs, BuffOnKarma rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnKarma lhs, BuffOnKarma rhs) => !(lhs == rhs);
    }
}
