using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts
{
    public enum PersonalityTraits
    {
        All = -1,
        None = 0,

        pragmatism,//abstraction
        Imagination,//abstraction

        secure,//concern
        Concerned,//concern

        submissive,//dominance
        Dominant,//dominance

        unstable,//emotionalstability
        Calm,//emotionalstability

        content,//liveliness
        Spontaneous,//liveliness

        attachedToTheFamiliar,//opennessToChange
        Flexible,//opennessToChange


        undisciplined,//perfectionism
        Controlled,//perfectionism

        open,//privacy
        Discreet,//privacy

        concrete,//reasoning
        Abstract,//reasoning


        nonConforming,//normativeConsciousness
        ConformingToStandards,//normativeConsciousness

        dependency,//selfConfidence
        SelfSufficiency,//selfConfidence


        toughness,//sensitivity
        Sweetness,//sensitivity


        shy,//socialdaring
        Uninhibited,//socialdaring


        relaxed,//tension
        Impatient,//tension

        confident,//alertness
        Distrustful,//alertness


        reserved,//warmth
        Outgoing,//warmth


        sensitive,//resilience
        Resilient//resilience
        

    }
    public enum HalfPersonalityTraits
    {
        /// <summary>
        /// pragmatism / Imagination
        /// </summary>
        abstraction = 1,
        /// <summary>
        ///: secure / Concerned
        /// </summary>
        concern = 2,
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
