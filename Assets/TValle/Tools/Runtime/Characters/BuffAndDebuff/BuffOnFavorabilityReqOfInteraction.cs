using Assets.TValle.Tools.Runtime.Characters.Atts;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Intections;
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
    public struct BuffOnFavorabilityReqOfInteraction : IIdentifiableBuff<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, SimpleModifier, Operation, int)>,
        IStackableBuff<BuffOnFavorabilityReqOfInteraction>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;

        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;
        public bool isValid => interationReceivedType != InterationReceivedType.None && fromPart != TriggeringBodyPart.None && toPart != SensitiveBodyPart.None && modifier != SimpleModifier.None && operation != Operation.None && endHour != 0 && float.IsFinite(value);

        public string DebugPrint()
        {
            return interationReceivedType.ToString() + "->" + fromPart.ToString() + "->" + toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => DisplayableBuffCategory.favorability;
        public string RichPrint(Func<string, string> characterNameGetter, Language language)//
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(fromPart, language) + " " + 
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(interationReceivedType, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(toPart, language) + " " + 
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " + 
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Favorability Req. " + RichPrint(characterNameGetter, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, SimpleModifier, Operation, int) valueId => (interationReceivedType, fromPart, toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnFavorabilityReqOfInteraction Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        //public BuffOnFavorabilityReqOfInteraction StackToNew(ref BuffOnFavorabilityReqOfInteraction Other)
        //{
        //    var r = this;
        //    switch(operation)
        //    {
        //        case Operation.None:
        //            break;
        //        case Operation.mult:
        //            r.value *= Other.value;
        //            break;
        //        case Operation.add:
        //            r.value += Other.value;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(operation.ToString());
        //    }
        //    return r;
        //}

        public void StackToSelf(ref BuffOnFavorabilityReqOfInteraction Other)
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
                case Operation.mult:
                    value *= Other.value;
                    break;
                case Operation.add:
                    value += Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }
        public void StackToSelf(object Other)
        {
            if(!(Other is BuffOnFavorabilityReqOfInteraction))
            {
                return;
            }
            var OtherBuff = (BuffOnFavorabilityReqOfInteraction)Other;
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


        public override bool Equals(object obj) => this.Equals((BuffOnFavorabilityReqOfInteraction)obj);
        public bool Equals(BuffOnFavorabilityReqOfInteraction p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnFavorabilityReqOfInteraction lhs, BuffOnFavorabilityReqOfInteraction rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnFavorabilityReqOfInteraction lhs, BuffOnFavorabilityReqOfInteraction rhs) => !(lhs == rhs);
    }
}
