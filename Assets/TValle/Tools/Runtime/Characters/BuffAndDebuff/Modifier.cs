using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public enum SimpleModifier
    {
        None = 0,
        value,
    }
    public enum EmotionModifier
    {
        None = 0,
        defaultValue, 
        minValue,
        maxValue,
        gain,
    }
    public enum InteractionModifier 
    {
        None = 0,
        damage,
        gainIntervalExpand,
        gainIntervalPosition,

    }
}
