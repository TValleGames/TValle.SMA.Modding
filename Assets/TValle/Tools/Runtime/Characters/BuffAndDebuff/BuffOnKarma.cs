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
    public struct BuffOnKarma : IIdentifiableBuff<(SimpleModifier, Operation, int)>, IStackableBuff<BuffOnKarma>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff, IValidableBuff
    {
        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;
        public bool isValid => modifier != SimpleModifier.None && operation != Operation.None && endHour != 0;

        public string DebugPrint()
        {
            return modifier.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint(Func<string, string> characterNameGetter, Language language)
        {
            var r =  TValleUILocalTextAttribute.LocalizadoFirstCharToUpper(modifier, language) + " " +
                operation.GetOperationSymbol(value) + value.ToString("0.00");
            return r;
        }

        public bool infinite => endHour < 0;
        public DateTime endTime => infinite ? DateTime.MaxValue : DateTime.MinValue.AddHours(endHour);
        public (SimpleModifier, Operation, int) valueId => (modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;



        public bool IsStackableWith(ref BuffOnKarma Other)
        {
            return Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnKarma StackToNew(ref BuffOnKarma Other)
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

        public void StackToSelf(ref BuffOnKarma Other)
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



        public override bool Equals(object obj) => this.Equals((BuffOnKarma)obj);
        public bool Equals(BuffOnKarma p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffOnKarma lhs, BuffOnKarma rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffOnKarma lhs, BuffOnKarma rhs) => !(lhs == rhs);
    }
}
