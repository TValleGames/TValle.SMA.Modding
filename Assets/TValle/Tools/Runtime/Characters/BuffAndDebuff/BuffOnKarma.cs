﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffOnKarma : IIdentifiableBuff<(SimpleModifier, Operation, int)>, IStackableBuff<BuffOnKarma>, IFloatValuableBuff, IEndableOnDateBuff
    {
        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;


        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (SimpleModifier, Operation, int) valueId => (modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;



        public bool IsStackableWith(ref BuffOnKarma Other)
        {
            return Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnKarma StackToNew(ref BuffOnKarma Other)
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

        public void StackToSelf(ref BuffOnKarma Other)
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