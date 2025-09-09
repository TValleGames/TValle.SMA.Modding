using Assets.TValle.Tools.Runtime.Characters.Atts;
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
    public struct BuffOnPersonalityTrait : IIdentifiableBuff<(PersonalityTraits, SimpleModifier, Operation, int)>, IStackableBuff<BuffOnPersonalityTrait>,
        IEquatable<BuffOnPersonalityTrait>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public PersonalityTraits trait;

        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;

        public bool isValid => trait != PersonalityTraits.None && modifier != SimpleModifier.None && operation != Operation.None && endHour != 0;

        public string DebugPrint()
        {
            return trait.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => DisplayableBuffCategory.other;
        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(trait, language) + " " + 
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " + 
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Personality Trait " + RichPrint(characterNameGetter, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (PersonalityTraits, SimpleModifier, Operation, int) valueId => (trait, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(object Other)
        {
            if(!(Other is BuffOnPersonalityTrait))
            {
                return false;
            }
            var OtherBuff = (BuffOnPersonalityTrait)Other;
            return IsStackableWith(ref OtherBuff);
        }
        public bool IsStackableWith(ref BuffOnPersonalityTrait Other)
        {
            return Other.trait == trait && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        //public BuffOnPersonalityTrait StackToNew(ref BuffOnPersonalityTrait Other)
        //{
        //    var r = this;
        //    switch(operation)
        //    {
        //        case Operation.None:
        //            break;
        //        case Operation.add:
        //            r.value += Other.value;
        //            break;
        //        case Operation.mult:
        //            r.value *= Other.value;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(operation.ToString());
        //    }
        //    return r;
        //}

        public void StackToSelf(ref BuffOnPersonalityTrait Other)
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
        public void StackToSelf(object Other)
        {
            if(!(Other is BuffOnPersonalityTrait))
            {
                return;
            }
            var OtherBuff = (BuffOnPersonalityTrait)Other;
            StackToSelf(ref OtherBuff);
        }
        public void InverseValue()
        {
            if(value == 0)
                return;
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.add:
                    value = -value;
                    break;
                case Operation.mult:
                    value = 1f / value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }





        public override bool Equals(object obj) => this.Equals((BuffOnPersonalityTrait)obj);
        public bool Equals(BuffOnPersonalityTrait p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnPersonalityTrait lhs, BuffOnPersonalityTrait rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnPersonalityTrait lhs, BuffOnPersonalityTrait rhs) => !(lhs == rhs);
    }
}
