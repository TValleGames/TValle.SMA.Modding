﻿using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public struct BuffOnHoleWearingWalls : IIdentifiableBuff<(SensitiveFemaleHoleWalls, SimpleModifier, AddOperation, int)>, IStackableBuff<BuffOnHoleWearingWalls>,
        IEquatable<BuffOnHoleWearingWalls>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff
    {
        public SensitiveFemaleHoleWalls toPart;
        public SimpleModifier modifier;
        public AddOperation operation;
        public int endHour;
        public float value;

        public string DebugPrint()
        {
            return toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint()
        {
            throw new NotImplementedException();
        }

        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (SensitiveFemaleHoleWalls, SimpleModifier, AddOperation, int) valueId => (toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnHoleWearingWalls Other)
        {
            return Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnHoleWearingWalls StackToNew(ref BuffOnHoleWearingWalls Other)
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

        public void StackToSelf(ref BuffOnHoleWearingWalls Other)
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





        public override bool Equals(object obj) => this.Equals((BuffOnHoleWearingWalls)obj);
        public bool Equals(BuffOnHoleWearingWalls p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnHoleWearingWalls lhs, BuffOnHoleWearingWalls rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnHoleWearingWalls lhs, BuffOnHoleWearingWalls rhs) => !(lhs == rhs);

    }
}