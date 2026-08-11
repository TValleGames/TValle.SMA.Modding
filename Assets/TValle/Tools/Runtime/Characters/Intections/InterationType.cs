using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.TValle.Tools.Runtime.Characters.Intections
{
    [TValleDoLocalizeEnum]
    public enum InterationReceivedType
    {
        All = -1,
        None = 0,
        [Label("look At", Language.en)]
        lookAt,
        photoshoot,
        [Label("put In Front", Language.en)]
        putInFront,

        caress,
        kiss,
        slap,
        hump,
        poke,
        [Label("dry Hump", Language.en)]
        dryhump,
        lick,
        [Label("pouring On", Language.en)]
        pouringOn,
        [Label("pouring In", Language.en)]
        pouringIn,
        punch,

        penetration,
        fingering,
        propped,

        expose,
        [Label("ask To Expose", Language.en)]
        askToExpose,
        [Label("force Pose", Language.en)]
        forcePose,
        [Label("ask To Pose", Language.en)]
        askToPose,
        [Label("manipulate Body", Language.en)]
        manipulateBody,
        [Label("guide Body", Language.en)]
        guideBody,



        handJob,


    }
    [TValleDoLocalizeEnum]
    public enum InterationDirectionType
    {
        None = 0,
        received,
        given,
    }
    [TValleDoLocalizeEnum]
    public enum IntercouseInterationReceivedType
    {
        None = 0,

        penetration = InterationReceivedType.penetration,
        fingering = InterationReceivedType.fingering,
        propped = InterationReceivedType.propped,
    }
    [TValleDoLocalizeEnum]
    public enum AllTocuhInterationReceivedType
    {
        None = 0,

        caress = InterationReceivedType.caress,
        kiss = InterationReceivedType.kiss,
        slap = InterationReceivedType.slap,
        hump = InterationReceivedType.hump,
        poke = InterationReceivedType.poke,
        [Label("dry Hump", Language.en)]
        dryhump = InterationReceivedType.dryhump,
        lick = InterationReceivedType.lick,
        [Label("pouring On", Language.en)]
        pouringOn = InterationReceivedType.pouringOn,
        [Label("pouring In", Language.en)]
        pouringIn = InterationReceivedType.pouringIn,
        punch = InterationReceivedType.punch,

        expose = InterationReceivedType.expose,
        [Label("force Pose", Language.en)]
        forcePose = InterationReceivedType.forcePose,
        [Label("manipulate Body", Language.en)]
        manipulateBody = InterationReceivedType.manipulateBody,

        handJob = InterationReceivedType.handJob,
    }
    public static class InterationReceivedTypeEXT
    {
        public static bool IsContextValid(this InterationReceivedType inter, Emotion emo)
        {
            switch(emo)
            {
                case Emotion.pain:
                {
                    switch(inter)
                    {
                        case InterationReceivedType.lookAt:
                        case InterationReceivedType.photoshoot:
                        case InterationReceivedType.putInFront:
                        case InterationReceivedType.pouringOn:
                        case InterationReceivedType.pouringIn:
                        case InterationReceivedType.askToExpose:
                        case InterationReceivedType.askToPose:
                        case InterationReceivedType.guideBody:
                            return false;
                        default:
                            return true;
                    }
                }
                default:
                    return true;
            }

        }
        public static bool TryInverse(this InterationReceivedType inter, out InterationReceivedType interInversed)
        {
            interInversed = inter;
            switch(inter)
            {
                case InterationReceivedType.All:
                case InterationReceivedType.None:
                    return false;
                case InterationReceivedType.handJob:
                    interInversed = InterationReceivedType.dryhump;
                    break;
                default:
                    break;
            }
            return true;
        }

    }
}