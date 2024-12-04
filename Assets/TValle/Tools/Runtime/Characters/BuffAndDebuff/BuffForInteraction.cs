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
    public struct BuffForInteraction : IIdentifiableBuff<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int)>, IStackableBuff<BuffForInteraction>
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public Emotion emotion;

        public InteractionModifier modifier;
        public ProductOperation operation;
        public int durationInDays;
        public float value;




        public (InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int) valueId => (interationReceivedType, fromPart, toPart, emotion, modifier, operation, durationInDays);
        public ITuple id => valueId;
        public string stringId => valueId.ToString();



        public bool IsStackableWith(ref BuffForInteraction Other)
        {
            return

                Other.interationReceivedType == interationReceivedType && Other.fromPart == fromPart && Other.toPart == toPart &&
                Other.emotion == emotion && Other.modifier == modifier && Other.operation == operation && Other.durationInDays == durationInDays;
        }

        public BuffForInteraction StackToNew(ref BuffForInteraction Other)
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

        public void StackToSelf(ref BuffForInteraction Other)
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



        public override bool Equals(object obj) => this.Equals((BuffForInteraction)obj);
        public bool Equals(BuffForInteraction p)
        {
            return IsStackableWith(ref p);
        }
        public override int GetHashCode() => valueId.GetHashCode();
        public static bool operator ==(BuffForInteraction lhs, BuffForInteraction rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BuffForInteraction lhs, BuffForInteraction rhs) => !(lhs == rhs);
    }
}
