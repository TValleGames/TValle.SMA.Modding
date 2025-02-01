using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IEndableOnDateBuff
    {
        bool infinite { get; }
        DateTime endTime { get; }

    }

    public static class IEndableOnDateBuffExt
    {
        public static int DaysToBuffEndHour(this DateTime now, int daysToAdd)
        {
            var endTime = now.AddDays(daysToAdd);
            var lapse = endTime - DateTime.MinValue;
            return Convert.ToInt32(lapse.TotalHours);
        }
    }

}
