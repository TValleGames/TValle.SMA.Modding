﻿using Assets.TValle.Tools.Runtime.Characters.Atts;
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
    public struct BuffOnInteraction : IIdentifiableBuff<(InterationReceivedType, TriggeringBodyPart, SensitiveBodyPart, Emotion, InteractionModifier, ProductOperation, int)>, IStackableBuff<BuffOnInteraction>, IFloatValuableBuff, IEndableOnDateBuff
    {
        public InterationReceivedType interationReceivedType;
        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public Emotion emotion;

        public InteractionModifier modifier;
        public ProductOperation operation;
        public int endHour;
        public float value;



        public DateTime endTime => DateTime.MinValue.AddHours(endHour);
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