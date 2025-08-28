using Assets.TValle.Tools.Runtime.UI;
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
        [Label("default Value", Language.en)]
        defaultValue,
        [Label("min Value", Language.en)]
        minValue,
        [Label("max Value", Language.en)]
        maxValue,
        gain,
    }
    public enum SimpleEmotionModifier
    {
        None = 0, 
        [Label("default Value", Language.en)]
        defaultValue,
        gain,
    }
    public enum InteractionModifier 
    {
        None = 0,
        damage,
        [Label("gain Interval Expand", Language.en)]
        gainIntervalExpand,
      
        [Label("gain Interval Position", Language.en)]
        gainMinMaxIntervalPosition,

        [Label("gain Min Interval Position", Language.en)]
        gainMinIntervalPosition,

        [Label("gain Max Interval Position", Language.en)]
        gainMaxIntervalPosition,

    }
    //public enum FavorabilityModifier
    //{
    //    None = 0,
    //    req,

    //}
}
