using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts.Emotions
{
    public enum TriggeringBodyPart
    {
        All = -1,
        None = 0,

        [Label("not Specified", Language.en)]
        notSpecified,
        eyes,
        mouth,
        torso,
        hand,
        finger,
        leg,
        tongue,
        penis,
        [Label("tool", Language.en)]
        toy,
        semen,
    }
    public enum TriggeringPenetratingBodyPart
    {
        None = 0,
        finger = TriggeringBodyPart.finger,
        tongue = TriggeringBodyPart.tongue,
        penis = TriggeringBodyPart.penis,
        toy = TriggeringBodyPart.toy,

    }
    public static class TriggeringBodyPartHelper
    {
        static TriggeringBodyPartHelper()
        {
            {
                var p = new List<TriggeringBodyPart>();
                p.Add(TriggeringBodyPart.penis);
                p.Add(TriggeringBodyPart.finger);
                p.Add(TriggeringBodyPart.toy);
                p.Add(TriggeringBodyPart.tongue);
                canPenetrateParts = p;
            }
        }


        public static readonly IReadOnlyList<TriggeringBodyPart> canPenetrateParts;
    }
}
