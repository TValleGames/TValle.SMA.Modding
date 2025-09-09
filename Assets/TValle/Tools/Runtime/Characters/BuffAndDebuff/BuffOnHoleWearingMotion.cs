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

        public bool isValid => toPart != SensitiveFemaleHole.None && modifier != SimpleModifier.None && operation != AddOperation.None && endHour != 0 && float.IsFinite(value);

        public string DebugPrint()
        {
            return toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => DisplayableBuffCategory.other;
        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(toPart, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " + 
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Loose-Stroked " + RichPrint(characterNameGetter, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SensitiveFemaleHole, SimpleModifier, AddOperation, int) valueId => (toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;

        public bool IsStackableWith(object Other)
        {
            if(!(Other is BuffOnHoleWearingMotion))
            {
                return false;
            }
            var OtherBuff = (BuffOnHoleWearingMotion)Other;
            return IsStackableWith(ref OtherBuff);
        }
        public bool IsStackableWith(ref BuffOnHoleWearingMotion Other)
        {
            return Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        //public BuffOnHoleWearingMotion StackToNew(ref BuffOnHoleWearingMotion Other)
        //{
        //    var r = this;
        //    switch(operation)
        //    {
        //        case AddOperation.None:
        //            break;
        //        case AddOperation.add:
        //            r.value += Other.value;
        //            break;                
        //        default:
        //            throw new ArgumentOutOfRangeException(operation.ToString());
        //    }
        //    return r;
        //}

        public void StackToSelf(ref BuffOnHoleWearingMotion Other)
        {
            if(!Other.ValueIsValid())
            {
#if UNITY_EDITOR
                Debug.LogError("other buff " + this.GetType().Name + ", to be stacked upon has invalid value, buff: " + Other.DebugPrint());
#endif
                return;
            }
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
        public void StackToSelf(object Other)
        {
            if(!(Other is BuffOnHoleWearingMotion))
            {
                return;
            }
            var OtherBuff = (BuffOnHoleWearingMotion)Other;
            StackToSelf(ref OtherBuff);
        }
        public void InverseValue()
        {
            if(value == 0)
                return;
            switch(operation)
            {
                case AddOperation.None:
                    break;
                case AddOperation.add:
                    value = -value;
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
