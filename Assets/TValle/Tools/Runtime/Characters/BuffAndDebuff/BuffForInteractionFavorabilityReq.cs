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
    public struct BuffForInteractionFavorabilityReq : IIdentifiableBuff<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, SimpleModifier, ProductOperation, int)>, IStackableBuff<BuffForInteractionFavorabilityReq>
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public Emotion emotion;

        public SimpleModifier modifier;
        public ProductOperation operation;
        public int durationInDays;
        public float value;




        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, SimpleModifier, ProductOperation, int) valueId => (interationReceivedType, fromPart, toPart, emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();



        public bool IsStackableWith(ref BuffForInteractionFavorabilityReq Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart &&
                Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffForInteractionFavorabilityReq StackToNew(ref BuffForInteractionFavorabilityReq Other)
        {
            var r = this;
            switch(operation)
            {
                case ProductOperation.None:
                    break;
                case ProductOperation.mult:
                case ProductOperation.divide:
                    r.value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
            return r;
        }

        public void StackToSelf(ref BuffForInteractionFavorabilityReq Other)
        {
            switch(operation)
            {
                case ProductOperation.None:
                    break;
                case ProductOperation.mult:
                case ProductOperation.divide:
                    value *= Other.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(operation.ToString());
            }
        }



        public override bool Equals(object obj) => this.Equals((BuffForInteractionFavorabilityReq)obj);
        public bool Equals(BuffForInteractionFavorabilityReq p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffForInteractionFavorabilityReq lhs, BuffForInteractionFavorabilityReq rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffForInteractionFavorabilityReq lhs, BuffForInteractionFavorabilityReq rhs) => !(lhs == rhs);
    }
}
