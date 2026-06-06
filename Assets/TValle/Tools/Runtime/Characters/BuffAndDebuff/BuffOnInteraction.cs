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
        IStackableBuff<BuffOnInteraction>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff, IContextValidableBuff
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public Emotion emotion;

        public InteractionModifier modifier;
        public ProductOperation operation;
        public int endHour;
        public float value;

        public bool isValid => emotion != Emotion.None && interationReceivedType != InterationReceivedType.None && fromPart != TriggeringBodyPart.None && toPart != SensitiveBodyPart.None && modifier != InteractionModifier.None && operation != ProductOperation.None && endHour != 0 && float.IsFinite(value);
        public bool isContextValid => interationReceivedType.IsContextValid(emotion);



        public string DebugPrint()
        {
            return interationReceivedType.ToString() + "->" + fromPart.ToString() + "->" + toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => emotion.ParseToCategory();
        public string RichPrint(Func<string, string> characterNameGetter, float UIValue, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(fromPart, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(interationReceivedType, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(toPart, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(emotion, language) + " " +
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " +
                 //operation.GetOperationSymbol(UIValue) + UIValue.ToString("0.00");
                 operation.GetOperationSymbolAndValue(UIValue);
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Interaction " + RichPrint(characterNameGetter, value, language);
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int) valueId => (interationReceivedType, fromPart, toPart, emotion, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;

        public bool IsStackableWith(object Other)
        {
            if(!(Other is BuffOnInteraction))
            {
                return false;
            }
            var OtherBuff = (BuffOnInteraction)Other;
            return IsStackableWith(ref OtherBuff);
        }
        public bool IsStackableWith(ref BuffOnInteraction Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart &&
                Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        //public BuffOnInteraction StackToNew(ref BuffOnInteraction Other)
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

        public void StackToSelf(ref BuffOnInteraction Other)
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


        public bool ValueIsEmpty()
        {
            switch(operation)
            {
                case ProductOperation.None:
                    return true;
                case ProductOperation.mult:
                    return Mathf.Approximately(value, 1f);
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }
        public bool ValueIsDisplayable()
        {
            if(ValueIsEmpty())
                return false;
            switch(operation)
            {
                case ProductOperation.None:
                    return false;

                case ProductOperation.mult:
                    return Mathf.Abs(value - 1f) > 0.001f;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }
        public int ValuePriorty()
        {
            if(ValueIsEmpty())
                return 0;

            
            var pola = emotion.IsGood() ? 1f : -1f;

            switch(operation)
            {
                case ProductOperation.None:
                    return 0;

                case ProductOperation.mult:
                {
                    switch(modifier)
                    {
                        case InteractionModifier.None:
                            return 0;
                        case InteractionModifier.damage:
                            return this.CalcMultiplyValuePriority(-33 * pola, 33 * pola);
                        case InteractionModifier.gainIntervalExpand:
                            return this.CalcMultiplyValuePriority(-33 * pola, 33 * pola);
                        case InteractionModifier.gainMinMaxIntervalPosition:
                            return this.CalcMultiplyValuePriority(33 * pola, -33 * pola);
                        case InteractionModifier.gainMinIntervalPosition:
                            return this.CalcMultiplyValuePriority(33 * pola, -33 * pola);
                        case InteractionModifier.gainMaxIntervalPosition:
                            return this.CalcMultiplyValuePriority(33 * pola, -33 * pola);
                        default:
                            throw new ArgumentOutOfRangeException(modifier.ToString());
                    }
                }

                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }

        public static bool operator ==(BuffOnInteraction lhs, BuffOnInteraction rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnInteraction lhs, BuffOnInteraction rhs) => !(lhs == rhs);
    }
}
