using Assets.TValle.Tools.Runtime.Characters.Atts;
using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Intections;
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
        IStackableBuff<BuffOnFavorabilityReqOfInteraction>, IFloatValuableBuff, IEndableOnDateBuff, IPrintableBuff
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;

        public SimpleModifier modifier;
        public Operation operation;
        public int endHour;
        public float value;

        public string DebugPrint()
        {
            return interationReceivedType.ToString() + "->" + fromPart.ToString() + "->" + toPart.ToString() + "->" + modifier.ToString() + "->" + operation.ToString() + " End:" + (endHour < 0 ? "∞" : DateTime.MinValue.AddHours(endHour)) + " By:" + value.ToString();
        }

        public string RichPrint()
        {
            throw new NotImplementedException();
        }

        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, SimpleModifier, Operation, int) valueId => (interationReceivedType, fromPart, toPart, modifier, operation, endHour);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();
        public float buffValue => value;


        public bool IsStackableWith(ref BuffOnFavorabilityReqOfInteraction Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart && Other.modifier == modifier && Other.operation == operation && Other.endHour == endHour;
        }

        public BuffOnFavorabilityReqOfInteraction StackToNew(ref BuffOnFavorabilityReqOfInteraction Other)
        {
            var r = this;
            switch(operation)
            {
                case Operation.None:
                    break;
                case Operation.mult:
                    r.value *= Other.value;
                    break;
                case Operation.add:
                    r.value += Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffOnFavorabilityReqOfInteraction Other)
        {
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
