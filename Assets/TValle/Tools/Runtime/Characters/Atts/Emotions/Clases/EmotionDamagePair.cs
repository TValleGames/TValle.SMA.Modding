using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Atts.Emotions
{
    [Serializable]
    public struct EmotionDamagePair 
    {
        public Emotion main;
        public EmotionPercentageRange mainRange;

        public Emotion secondary;
        public EmotionPercentageRange secondaryRange;


        public (Emotion, EmotionPercentageRange, Emotion, EmotionPercentageRange) valueId => (main, mainRange, secondary, secondaryRange);
        public ITuple id => valueId;

        public float damageScore => damageScoreTotal / (float)times;

        public int times;
        //public int maxValueTimes;
        public float damagePercentageTotal;
        public float damageScoreTotal;


        public void StackToSelf(ref EmotionDamagePair Other)
        {
            times += Other.times;
            //maxValueTimes += Other.maxValueTimes;
            damagePercentageTotal += Other.damagePercentageTotal;
            damageScoreTotal += Other.damageScoreTotal;
        }

    }
}
