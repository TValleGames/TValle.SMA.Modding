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
    public struct BuffOnHoleWearingBottom : IIdentifiableBuff<(SensitiveFemaleHoleBottom, SimpleModifier, AddOperation, int)>, IStackableBuff<BuffOnHoleWearingBottom>,
        IEquatable<BuffOnHoleWearingBottom>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public SensitiveFemaleHoleBottom toPart;
        public SimpleModifier modifier;
        public AddOperation operation;
        public int endHour;
        public float value;
        public bool isValid => toPart != SensitiveFemaleHoleBottom.None && modifier != SimpleModifier.None && operation != AddOperation.None && endHour != 0;

        public string DebugPrint()
        {
            return toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint()
        {
            throw new NotImplementedException();
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SensitiveFemaleHoleBottom, SimpleModifier, AddOperation, int) valueId => (toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnHoleWearingBottom Other)
        {
            return Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnHoleWearingBottom StackToNew(ref BuffOnHoleWearingBottom Other)
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

        public void StackToSelf(ref BuffOnHoleWearingBottom Other)
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





        public override bool Equals(object obj) => this.Equals((BuffOnHoleWearingBottom)obj);
        public bool Equals(BuffOnHoleWearingBottom p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnHoleWearingBottom lhs, BuffOnHoleWearingBottom rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnHoleWearingBottom lhs, BuffOnHoleWearingBottom rhs) => !(lhs == rhs);

    }
}
