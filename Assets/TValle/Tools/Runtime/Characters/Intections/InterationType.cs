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
        putInFront,

        caress,
        kiss,
        slap,
        hump,
        poke,
        lick,
        pouringOn,
        pouringIn,
        punch,

        penetration,
        fingering,
        propped,

        expose,
        askToExpose,
        forcePose,
        askToPose,
        manipulateBody,
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
