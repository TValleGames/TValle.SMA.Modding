using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Intections
{
    [Serializable]
    public struct Interaction
    {
        /// <summary>
        /// How much damage would this interaction cause if it lasted a second?
        /// </summary>
        public float damagePercentageVelocity => duration == 0 ? 0 : damagePercentage / duration;
        public bool isValid => from != null && to != null && toPart != SensitiveBodyPart.None && fromPart != TriggeringBodyPart.None && interationReceivedType != InterationReceivedType.None && emotion != Emotion.None && times > 0;

        public SceneCharacter from;
        public SceneCharacter to;

        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public InterationReceivedType interationReceivedType;
        public Emotion emotion;


        /// <summary>
        /// The in-game date on which the scene occurs.
        /// </summary>
        public DateTime date;
        /// <summary>
        /// The time in seconds since the beginning of the scene when the interaction occurs.
        /// </summary>
        public float time;
        /// <summary>
        /// If the interaction is currently taking place, this value indicates how many frames have passed since it began. If this is an archived past interaction, this value represents the number of times it occurred in the scene.
        /// </summary>
        public int times;
        /// <summary>
        /// The duration of the interaction
        /// </summary>
        public float duration;
        /// <summary>
        /// This is true as long as the emotion is at its maximum value.
        /// </summary>
        public bool emotionAtMaxValue;
        /// <summary>
        /// This is only true if the interaction causes the emotion to reach its maximum value.
        /// <para>EX: To find out how many orgasms the "to" character receives from the "from" character, find every pleasure interaction with "reachedMaxValue" set to true.</para>
        /// <para>A pleasure interaction is just an interaction with emotion field set to pleasure.</para>
        /// </summary>
        public bool reachedMaxValue;
        /// <summary>
        /// This interaction results in a change in the target emotion, which is express here as a percentage.
        /// </summary>
        public float damagePercentage;

        ///// <summary>
        ///// It depends on the interaction context; it can mean speed, distance, angle, etc.
        ///// </summary>
        //public float stimulus;
        ///// <summary>
        ///// This is the minimum amount of stimulus the "to" character can receive in this interaction context.
        ///// </summary>
        //public float minRange;
        ///// <summary>
        ///// This is the maximum amount of stimulus the "to" character can receive in this interaction context.
        ///// </summary>
        //public float maxRange;
        /// <summary>
        /// how many times the stimulus amount is surpassed or how many times the stimulus amount is insufficient, zero if no damage was done
        /// <para>EX: This value is one if the caress's force falls within the appropriate range.</para>
        /// <para>EX: When performing a caress with excessive force, the value will exceed one, indicating the number of times the force exceeds the appropriate amount.</para>
        /// <para>EX: When performing a caress that is too soft below the appropriate range, this value will be less than one, indicating the fraction of the range that was reached below the appropriate range.</para>
        /// <para>This same logic applies to other types of interactions, such as penetration. This refers to the overshoot or undershoot of the length or width of the member inside a hole.</para>
        /// </summary>
        public float overshootOrUndershootMod;


    }
}
