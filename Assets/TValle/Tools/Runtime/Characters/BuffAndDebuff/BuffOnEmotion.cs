﻿using Assets.TValle.Tools.Runtime.Characters.Atts;
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
    public struct BuffOnEmotion : IIdentifiableBuff<(Emotion, EmotionModifier, Operation, int)>, IStackableBuff<BuffOnEmotion>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public Emotion emotion;

        public EmotionModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;

        public bool isValid => emotion != Emotion.None && modifier != EmotionModifier.None && operation != Operation.None && endHour != 0;

        public string DebugPrint()
        {
            return emotion.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }
        public DisplayableBuffCategory category => emotion.ParseToCategory();
        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r = TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(emotion, language) + " " + 
                TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " + 
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }
        public string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language)
        {
            return "Feelings " + RichPrint(characterNameGetter, language);
        }



        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (Emotion, EmotionModifier, Operation, int) valueId => (emotion, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;

        public bool IsStackableWith(ref BuffOnEmotion Other)
        {
            return Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnEmotion StackToNew(ref BuffOnEmotion Other)
        {
            var r = this;
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.add:
                    r.value += Other.value;
                    break;
                case Operation.mult:
                    r.value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffOnEmotion Other)
        {
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
            if(!(Other is BuffOnEmotion))
            {
                return;
            }
            var OtherBuff = (BuffOnEmotion)Other;
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


        public override bool Equals(object obj) => this.Equals((BuffOnEmotion)obj);
        public bool Equals(BuffOnEmotion p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnEmotion lhs, BuffOnEmotion rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnEmotion lhs, BuffOnEmotion rhs) => !(lhs == rhs);
    }
}
