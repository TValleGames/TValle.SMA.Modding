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
        [Label("toy", Language.en)]
        toy,//a tool that is penetrating a hole, turns/is a toy
        semen,

        vagina,
        anus,

        water,
        lubricant,
        orine,

        [Label("applicator", Language.en)]
        toyApplicator,//is a toy that ejaculates (water lube etc)
        [Label("tool", Language.en)]
        tool,//a prop
    }
    public enum TriggeringPenetratingBodyPart
    {
        None = 0,
        finger = TriggeringBodyPart.finger,
        tongue = TriggeringBodyPart.tongue,
        penis = TriggeringBodyPart.penis,
        toy = TriggeringBodyPart.toy,
        toyApplicator = TriggeringBodyPart.toyApplicator,
        tool = TriggeringBodyPart.tool,
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
                p.Add(TriggeringBodyPart.toyApplicator);
                p.Add(TriggeringBodyPart.tongue);
                p.Add(TriggeringBodyPart.tool);
                canPenetrateParts = p.ToHashSet().ToArray();
            }
            {
                var p = new List<TriggeringBodyPart>();
                p.Add(TriggeringBodyPart.penis);
                p.Add(TriggeringBodyPart.finger);
                p.Add(TriggeringBodyPart.toy);
                p.Add(TriggeringBodyPart.toyApplicator);
                p.Add(TriggeringBodyPart.tongue);
                p.Add(TriggeringBodyPart.tool);

                p.Add(TriggeringBodyPart.torso);
                p.Add(TriggeringBodyPart.hand);

                p.Add(TriggeringBodyPart.leg);
                p.Add(TriggeringBodyPart.semen);

                p.Add(TriggeringBodyPart.vagina);
                p.Add(TriggeringBodyPart.anus);

                p.Add(TriggeringBodyPart.water);
                p.Add(TriggeringBodyPart.lubricant);
                p.Add(TriggeringBodyPart.orine);

                canTouchParts = p.ToHashSet().ToArray();
            }
        }


        public static readonly IReadOnlyList<TriggeringBodyPart> canPenetrateParts;
        public static readonly IReadOnlyList<TriggeringBodyPart> canTouchParts;


        public static bool TryInverse(this TriggeringBodyPart parte, out SensitiveBodyPart sensitiveBodyPart)
        {
            sensitiveBodyPart = SensitiveBodyPart.None;
            switch(parte)
            {
                case TriggeringBodyPart.semen:
                case TriggeringBodyPart.toy:
                case TriggeringBodyPart.tool:
                case TriggeringBodyPart.toyApplicator:
                case TriggeringBodyPart.notSpecified:
                case TriggeringBodyPart.All:
                case TriggeringBodyPart.None:
                case TriggeringBodyPart.water:
                case TriggeringBodyPart.lubricant:
                case TriggeringBodyPart.orine:
                    return false;


                case TriggeringBodyPart.eyes:
                    sensitiveBodyPart = SensitiveBodyPart.eyes;
                    break;
                case TriggeringBodyPart.mouth:
                    sensitiveBodyPart = SensitiveBodyPart.throat;
                    break;
                case TriggeringBodyPart.torso:
                    sensitiveBodyPart = SensitiveBodyPart.chest;
                    break;
                case TriggeringBodyPart.hand:
                    sensitiveBodyPart = SensitiveBodyPart.hands;
                    break;
                case TriggeringBodyPart.finger:
                    sensitiveBodyPart = SensitiveBodyPart.hands;
                    break;
                case TriggeringBodyPart.leg:
                    sensitiveBodyPart = SensitiveBodyPart.legs;
                    break;
                case TriggeringBodyPart.tongue:
                    sensitiveBodyPart = SensitiveBodyPart.tongue;
                    break;
                case TriggeringBodyPart.penis:
                    sensitiveBodyPart = SensitiveBodyPart.clitorisOrPenis;
                    break;
                case TriggeringBodyPart.vagina:
                    sensitiveBodyPart = SensitiveBodyPart.vag;
                    break;
                case TriggeringBodyPart.anus:
                    sensitiveBodyPart = SensitiveBodyPart.anus;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(parte.ToString());
            }
            return true;
        }
    }
}
