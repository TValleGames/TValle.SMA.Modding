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
    public struct BuffOnInteraction : IIdentifiableBuff<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int)>,
        IStackableBuff<BuffOnInteraction>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public Emotion emotion;

        public InteractionModifier modifier;
        public ProductOperation operation;
        public int endHour;
        public float value;

        public bool isValid => emotion != Emotion.None && interationReceivedType != InterationReceivedType.None && fromPart != TriggeringBodyPart.None && toPart != SensitiveBodyPart.None && modifier != InteractionModifier.None && operation != ProductOperation.None && endHour != 0;

        public string DebugPrint()
        {
            return interationReceivedType.ToString() + "->" + fromPart.ToString() + "->" + toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => emotion.ParseToCategory();
        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(fromPart, language) + " " + 
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(interationReceivedType, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(toPart, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(emotion, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " + 
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Interaction " + RichPrint(characterNameGetter, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int) valueId => (interationReceivedType, fromPart, toPart, emotion, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnInteraction Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart &&
                Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnInteraction StackToNew(ref BuffOnInteraction Other)
        {
            var r = this;
            switch(operation)
            {
                case ProductOperation.None:
                    break;
                case ProductOperation.mult:
                    r.value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffOnInteraction Other)
        {
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
            if(!(Other is BuffOnInteraction))
            {
                return;
            }
            var OtherBuff = (BuffOnInteraction)Other;
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


        public override bool Equals(object obj) => this.Equals((BuffOnInteraction)obj);
        public bool Equals(BuffOnInteraction p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnInteraction lhs, BuffOnInteraction rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnInteraction lhs, BuffOnInteraction rhs) => !(lhs == rhs);
    }
}
