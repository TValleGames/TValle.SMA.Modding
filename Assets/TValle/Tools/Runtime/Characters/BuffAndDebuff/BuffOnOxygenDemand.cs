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
    public struct BuffOnOxygenDemand : IIdentifiableBuff<(SimpleModifier, ProductOperation, int)>, IStackableBuff<BuffOnOxygenDemand>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public SimpleModifier modifier;
        public ProductOperation operation;
        public int endHour;
        public float value;
        public bool isValid => modifier != SimpleModifier.None && operation != ProductOperation.None && endHour != 0;
        public string DebugPrint()
        {
            return modifier.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => DisplayableBuffCategory.other; 
        
        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " +
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Fatigability " + RichPrint(characterNameGetter, language);
        }
        public bool infinite => endHour < 0; 
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SimpleModifier, ProductOperation, int) valueId => (modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;



        public bool IsStackableWith(object Other)
        {
            if(!(Other is BuffOnOxygenDemand))
            {
                return false;
            }
            var OtherBuff = (BuffOnOxygenDemand)Other;
            return IsStackableWith(ref OtherBuff);
        }
        public bool IsStackableWith(ref BuffOnOxygenDemand Other)
        {
            return Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }
        //public BuffOnOxygenDemand StackToNew(ref BuffOnOxygenDemand Other)
        //{
        //    var r = this;
        //    switch(operation)
        //    {
        //        case ProductOperation.None:
        //            break;               
        //        case ProductOperation.mult:
        //            r.value *= Other.value;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(operation.ToString());
        //    }
        //    return r;
        //}
        public void StackToSelf(ref BuffOnOxygenDemand Other)
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
                case ProductOperation.None:
                    break;
              
                case ProductOperation.mult:
                    value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }
        public void StackToSelf(object Other)
        {
            if(!(Other is BuffOnOxygenDemand))
            {
                return;
            }
            var OtherBuff = (BuffOnOxygenDemand)Other;
            StackToSelf(ref OtherBuff);
        }
        public void InverseValue()
        {
            if(value == 0)
                return;
            switch(operation)
            {
                case ProductOperation.None:
                    break;               
                case ProductOperation.mult:
                    value = 1f / value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }



        public override bool Equals(object obj) => this.Equals((BuffOnOxygenDemand)obj);
        public bool Equals(BuffOnOxygenDemand p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();



        public static bool operator ==(BuffOnOxygenDemand lhs, BuffOnOxygenDemand rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnOxygenDemand lhs, BuffOnOxygenDemand rhs) => !(lhs == rhs);


    }
}
