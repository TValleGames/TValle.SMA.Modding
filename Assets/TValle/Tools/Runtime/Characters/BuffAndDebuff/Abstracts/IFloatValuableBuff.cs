using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IFloatValuableBuff : IValuableBuff<float>
    {
        void InverseValue();
        bool ValueIsEmpty();
        bool ValueIsDisplayable();
        int ValuePriorty();

    }
    public static class IFloatValuableBuffEXT
    {
        public static bool ValueIsValid(this IFloatValuableBuff valuable)
        {
            return float.IsFinite(valuable.buffValue);
        }
        public static int CalcAddingValuePriority(this IFloatValuableBuff valuable, float worstSubs, float bestAdds)
        {
            return CalcValuePriority(valuable.buffValue, worstSubs, bestAdds);
        }
        public static int CalcMultiplyValuePriority(this IFloatValuableBuff valuable, float worstPercent, float bestPercent)
        {
            var percent = (valuable.buffValue - 1f) * 100f;
            return CalcValuePriority(percent, worstPercent, bestPercent);
        }
        static int CalcValuePriority(float value, float worst, float best, float @default = 0)
        {
            var t = InverseLerp(worst, @default, best, value);
            var q = t.LerpToItemQuality();
            var pol = q.Polarize();
            return pol;
        }
        static float InverseLerp(float min, float middle, float max, float value)
        {
            bool invert = min > max;

            if(invert)
            {
                var mi = min;
                var ma = max;
                min = ma;
                max = mi;
            }

            float r;

            if(value > middle)
                r = Mathf.Lerp(0.5f, 1, Mathf.InverseLerp(middle, max, value));
            else if(value < middle)
                r = Mathf.Lerp(0, 0.5f, Mathf.InverseLerp(min, middle, value));
            else
                r = 0.5f;

            if(invert)
                r = 1 - r;
            return r;
        }
    }
}
