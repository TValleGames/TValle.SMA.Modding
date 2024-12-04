using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts
{
    public enum PersonalityTraits 
    {
        /// <summary>
        /// pragmatism / Imagination
        /// </summary>
        abstraction = 1,
        /// <summary>
        ///: secure / Concerned
        /// </summary>
        preoccupied = 2,
        /// <summary>
        ///: submissive / Dominant
        /// </summary>
        dominance = 4,
        /// <summary>
        ///: unstable / Calm
        /// </summary>
        emotionalstability = 8,
        /// <summary>
        ///: content / Spontaneous
        /// </summary>
        liveliness = 16,
        /// <summary>
        ///: attached to the familiar / Flexible
        /// </summary>
        opennessToChange = 32,
        /// <summary>
        ///: undisciplined / Controlled
        /// </summary>
        perfectionism = 64,
        /// <summary>
        ///: open / Discreet
        /// </summary>
        privacy = 128,
        /// <summary>
        ///: concrete / Abstract
        /// </summary>
        reasoning = 256,
        /// <summary>
        ///: non-conforming / Conforming to standards
        /// </summary>
        normativeConsciousness = 512,
        /// <summary>
        ///: dependency / Self-sufficiency
        /// </summary>
        selfConfidence = 1024,
        /// <summary>
        ///: toughness / Warmth
        /// </summary>
        sensitivity = 2048,
        /// <summary>
        ///: shy / Uninhibited
        /// </summary>
        socialdaring = 4096,
        /// <summary>
        ///: relaxed / impatient
        /// </summary> ]
        tension = 8192,
        /// <summary>
        ///: confident / distrustful
        /// </summary>
        alertness = 16384,
        /// <summary>
        ///: reserved / outgoing
        /// </summary>
        warmth = 32768,
        /// <summary>
        /// sensitive / resilient
        /// </summary>
        resilience = 65536,

    }
}
