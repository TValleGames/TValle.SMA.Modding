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
    public struct BuffOnEyaculationAmount : IIdentifiableBuff<(SimpleModifier, ProductOperation, int)>, IStackableBuff<BuffOnEyaculationAmount>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff, IContextValidableBuff
    {
        public SimpleModifier modifier;
        public ProductOperation operation;
        public int endHour;
        public float value;

        public bool isValid => modifier != SimpleModifier.None && operation != ProductOperation.None && endHour != 0 && float.IsFinite(value);
        public bool isContextValid => true;
        public string DebugPrint()
        {
            return modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => DisplayableBuffCategory.pleasure;
        public string RichPrint(Func<string, string> characterNameGetter, float UIValue, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " +
                operation.GetOperationSymbol(UIValue) + UIValue.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Desires " + RichPrint(characterNameGetter, value, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SimpleModifier, ProductOperation, int) valueId => (modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;



        public bool IsStackableWith(object Other)
        {
            if(!(Other is BuffOnEyaculationAmount))
            {
                return false;
            }
            var OtherBuff = (BuffOnEyaculationAmount)Other;
            return IsStackableWith(ref OtherBuff);
        }
        public bool IsStackableWith(ref BuffOnEyaculationAmount Other)
        {
            return Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        

        public void StackToSelf(ref BuffOnEyaculationAmount Other)
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
                //case Operation.add:
                //    value += Other.value;
                //    break;
                case ProductOperation.mult:
                    value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }
        public void StackToSelf(object Other)
        {
            if(!(Other is BuffOnEyaculationAmount))
            {
                return;
            }
            var OtherBuff = (BuffOnEyaculationAmount)Other;
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
                //case Operation.add:
                //    value = -value;
                //    break;
                case ProductOperation.mult:
                    value = 1f / value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }


        public override bool Equals(object obj) => this.Equals((BuffOnEyaculationAmount)obj);
        public bool Equals(BuffOnEyaculationAmount p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();



        public static bool operator ==(BuffOnEyaculationAmount lhs, BuffOnEyaculationAmount rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnEyaculationAmount lhs, BuffOnEyaculationAmount rhs) => !(lhs == rhs);




    }
}
