using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class BatchCountEXT
{
    public static int BatchCountONE(this int arrayLength)
    {
        return Mathf.Clamp(arrayLength + 1, 1, int.MaxValue);
    }

    public static int BatchCountAllProcessors(this int arrayLength)
    {
        if(arrayLength == 0)
            return 1;
        var cantidadDeNucleos = SystemInfo.processorCount;
        var adding = arrayLength <= cantidadDeNucleos ? 0 : 1;

        return Mathf.Clamp((arrayLength / cantidadDeNucleos) + adding, 1, int.MaxValue);
    }

    public static int BatchCountAllProcessorsButOne(this int arrayLength)
    {
        if(arrayLength == 0)
            return 1;
        var cantidadDeNucleos = SystemInfo.processorCount - 1;
        return Mathf.Clamp((arrayLength / cantidadDeNucleos) + 1, 1, int.MaxValue);
    }
    public static int BatchCountLIGHT(this int arrayLength)
    {
        if(arrayLength == 0)
            return 1;
        var cantidadDeNucleos = Mathf.Clamp(SystemInfo.processorCount / 2, 1, int.MaxValue);
        return Mathf.Clamp((arrayLength / cantidadDeNucleos) + 1, 1, int.MaxValue);
    }
    public static int BatchCountSUPERLIGHT(this int arrayLength)
    {
        if(arrayLength == 0)
            return 1;
        var cantidadDeNucleos = Mathf.Clamp(SystemInfo.processorCount / 4, 1, int.MaxValue);
        return Mathf.Clamp((arrayLength / cantidadDeNucleos) + 1, 1, int.MaxValue);
    }

}

