using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.TValle.Tools.Runtime.Characters.Intections
{

    public enum InterationReceivedType
    {
        All = -1,
        None = 0,

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
    }
    public enum InterationDirectionType
    {
        None = 0,
        received,
        given,
    }
    public enum IntercouseInterationReceivedType
    {
        None = 0,

        penetration= InterationReceivedType.penetration,
        fingering= InterationReceivedType.fingering,
        propped = InterationReceivedType.propped,
    }

}
