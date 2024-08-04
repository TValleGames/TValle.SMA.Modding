using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts.Emotions
{
    [Flags]
    public enum TriggeringBodyPart
    {
        All = -1,
        None = 0,

        notSpecified,
        eyes,
        mouth,
        torso,
        hand,
        finger,
        leg,
        tongue,
        penis,
        toy,
        semen,
    }
}
