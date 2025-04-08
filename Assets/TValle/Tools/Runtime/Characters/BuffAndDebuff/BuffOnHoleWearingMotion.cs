using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    [Serializable]
    public struct BuffOnHoleWearingMotion : IIdentifiableBuff<(SensitiveFemaleHole, SimpleModifier, AddOperation, int)>, IStackableBuff<BuffOnHoleWearingMotion>,
        IEquatable<BuffOnHoleWearingMotion>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public SensitiveFemaleHole toPart;
        public SimpleModifier modifier;
        public AddOperation operation;
        public int endHour;
        public float value;

        public bool isValid => toPart != SensitiveFemaleHole.None && modifier != SimpleModifier.None && operation != AddOperation.None && endHour != 0;

        public string DebugPrint()
        {
            return toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.Localizado(toPart, language) + " " + TValleUILocalTextAttribute.Localizado(modifier, language) + " " + operation.GetOperationSymbol(value) + value.ToString();
            return r;
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SensitiveFemaleHole, SimpleModifier, AddOperation, int) valueId => (toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnHoleWearingMotion Other)
        {
            return Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnHoleWearingMotion StackToNew(ref BuffOnHoleWearingMotion Other)
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

        public void StackToSelf(ref BuffOnHoleWearingMotion Other)
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





        public override bool Equals(object obj) => this.Equals((BuffOnHoleWearingMotion)obj);
        public bool Equals(BuffOnHoleWearingMotion p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnHoleWearingMotion lhs, BuffOnHoleWearingMotion rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnHoleWearingMotion lhs, BuffOnHoleWearingMotion rhs) => !(lhs == rhs);

    }
}
