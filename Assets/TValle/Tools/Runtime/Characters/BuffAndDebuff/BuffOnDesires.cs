﻿using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    [Serializable]
    public struct BuffOnDesires : IIdentifiableBuff<(Desires, EmotionModifier, Operation, int)>, IStackableBuff<BuffOnDesires>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff
    {
        public Desires desires;
        public EmotionModifier modifier;
        public Operation operation;
        public int endHour;

        public float value;


        public string DebugPrint()
        {
            return desires.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint()
        {
            throw new NotImplementedException();
        }

        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (Desires, EmotionModifier, Operation, int) valueId => (desires, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;

        public bool IsStackableWith(ref BuffOnDesires Other)
        {
            return Other.desires == desires && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnDesires StackToNew(ref BuffOnDesires Other)
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

        public void StackToSelf(ref BuffOnDesires Other)
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



        public override bool Equals(object obj) => this.Equals((BuffOnDesires)obj);
        public bool Equals(BuffOnDesires p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();



        public static bool operator ==(BuffOnDesires lhs, BuffOnDesires rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnDesires lhs, BuffOnDesires rhs) => !(lhs == rhs);




    }
}