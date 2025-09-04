using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts.Emotions
{
    public enum Emotion
    {
        All = -1,
        None = 0,

        enjoyment,
        relief,
        favorability,
        pleasure,
        arousal,

        disappointment,
        rage,
        pain,
        fear,


        disgust,
    }
    public static class EmotionExt
    {
        public static readonly Emotion[] emotionsWithDefaultValueBuff = new Emotion[] { Emotion.pleasure, Emotion.enjoyment, Emotion.disappointment, Emotion.rage, Emotion.pain, Emotion.fear };
       
        
        
        
        public static readonly Emotion[] femaleEmotions = new Emotion[] { Emotion.arousal, Emotion.pleasure, Emotion.favorability, Emotion.relief, Emotion.enjoyment, Emotion.disappointment, Emotion.rage, Emotion.pain, Emotion.fear };

        public static bool IsGood(this Emotion emo)
        {
            switch(emo)
            {
                case Emotion.arousal:
                case Emotion.pleasure:
                case Emotion.favorability:
                case Emotion.relief:
                case Emotion.enjoyment:
                    return true;

                case Emotion.disappointment:
                case Emotion.rage:
                case Emotion.pain:
                case Emotion.fear:
                case Emotion.disgust:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(emo.ToString());
            }
        }
    }
}
