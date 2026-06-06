using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime
{
    public enum ItemQuality
    {
        None = 0,

        Doomed,
        Cursed,
        Haunted,
        Brittle,
        Defective,
        Poor,

        Common,

        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact,
        Relic,
    }

    public static class ItemQualityEXT
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">0.5 to ItemQuality.Common, 0 to ItemQuality.Doomed, 1 to ItemQuality.Relic  </param>
        /// <returns></returns>
        public static ItemQuality LerpToItemQuality(this float t)
        {
            var val = Lerp((int)ItemQuality.Doomed, (int)ItemQuality.Common, (int)ItemQuality.Relic, t);
            return (ItemQuality)Mathf.RoundToInt(val);
        }
        public static int Polarize(this ItemQuality itemQuality)
        {
            var val = (int)itemQuality - 7;
            return val;
        }


        static float Lerp(float a, float middle, float b, float t)
        {
            t = Mathf.Clamp01(t);
            if(t == 0.5f)
                return middle;
            if(t > 0.5f)
                return Mathf.Lerp(middle, b, Mathf.InverseLerp(0.5f, 1, t));
            if(t < 0.5f)
                return Mathf.Lerp(a, middle, Mathf.InverseLerp(0f, 0.5f, t));

            throw new ArgumentOutOfRangeException();
        }
    }
}
