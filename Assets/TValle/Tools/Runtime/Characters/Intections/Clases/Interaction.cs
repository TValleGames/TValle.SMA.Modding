using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Intections
{


    [Serializable]
    public struct InteractionToDisk
    {
        /// <summary>
        /// fromID
        /// </summary>
        public string fID;
        /// <summary>
        /// toID
        /// </summary>
        public string tID;
        /// <summary>
        /// fromPart
        /// </summary>
        public TriggeringBodyPart fP;
        /// <summary>
        /// toPart
        /// </summary>
        public SensitiveBodyPart tP;
        /// <summary>
        /// interationReceivedType
        /// </summary>
        public InterationReceivedType typ;
        /// <summary>
        /// emotion
        /// </summary>
        public Emotion emo;
        /// <summary>
        /// dateString
        /// </summary>
        public string dS;
        /// <summary>
        /// startTime
        /// </summary>
        public float sT;
        /// <summary>
        /// endTime
        /// </summary>
        public float eT;
        /// <summary>
        /// startFrame
        /// </summary>
        public int sF;
        /// <summary>
        /// endFrame
        /// </summary>
        public int eF;
        /// <summary>
        /// stacks 
        /// </summary>
        public int ts;
        /// <summary>
        /// times
        /// </summary>
        public int tss;
        /// <summary>
        /// damagePercentageDone
        /// </summary>
        public float dmg;
        /// <summary>
        /// emotionAtMaxValueTimes
        /// </summary>
        public int emoMxTs;
        /// <summary>
        /// triggerMaxValueTimes
        /// </summary>
        public int tggMxTs;
        /// <summary>
        /// overshootOrUndershootTotal
        /// </summary>
        public float off;
        /// <summary>
        /// damageScoreTotal
        /// </summary>
        public float dmgS;







        public Interaction ToInter()
        {
            Interaction r = new Interaction();
            r.fromID = fID;
            r.toID = tID;
            r.fromPart = fP;
            r.toPart = tP;
            r.interationReceivedType = typ;
            r.emotion = emo;
            r.dateString = dS;
            r.startTime = sT;
            r.endTime = eT;
            r.startFrame = sF;
            r.endFrame = eF;
            r.stacks = ts;
            r.times = tss;
            r.damagePercentageDone = dmg;
            r.emotionAtMaxValueTimes = emoMxTs;
            r.triggerMaxValueTimes = tggMxTs;
            r.overshootOrUndershootTotal = off;
            r.damageScoreTotal = dmgS;

            if(r.times <= 0)
                r.times = 1;
            if(r.stacks <= 0)
                r.stacks = 1;
            return r;
        }
    }
    [Serializable]
    public struct Interaction
    {
        /// <summary>
        /// Warning: The calculation of duration and frames will only be valid if the interactions belong to the same interaction recording.
        /// </summary>
        public static void AddFromDifferentRecordings(ref Interaction mutable, ref Interaction inmutable)
        {
            var dm = mutable.duration;
            var di = inmutable.duration;

            var fm = mutable.frames;
            var fi = inmutable.frames;


            StackFromSameRecording(ref mutable, ref inmutable, false, false);

            mutable.startTime = 0;
            mutable.startFrame = 0;
            mutable.endTime = dm + di;
            mutable.endFrame = fm + fi;


            mutable.times = mutable.times + inmutable.times;

            var m_d = mutable.date;
            var i_d = inmutable.date;

            mutable.date = (m_d < i_d) ? m_d : i_d;


        }

        /// <summary>
        /// Warning: The calculation of duration and frames will only be valid if the interactions belong to the same interaction recording.
        /// </summary>
        public static void AddFromSameRecording(ref Interaction toReport, ref Interaction newInteraccion)
        {
            StackFromSameRecording(ref toReport, ref newInteraccion, false, true);
            toReport.times = toReport.times + newInteraccion.times;

            var r_d = toReport.date;
            var n_d = newInteraccion.date;

            toReport.date = (r_d < n_d) ? r_d : n_d;

        }//

        /// <summary>
        /// Warning: The calculation of duration and frames will only be valid if the interactions belong to the same interaction recording.
        /// </summary>
        public static void StackFromSameRecording(ref Interaction toReport, ref Interaction newInteraccion, bool addTimes, bool fixStartTime)
        {
#if UNITY_EDITOR
            if(newInteraccion.damagePercentageDone < 0)
                Debug.LogError("A stacking to Interaction with negative values is occurring");
#endif
            if(addTimes)
                toReport.times = toReport.times + 1;

            toReport.stacks = toReport.stacks + 1;

            if(fixStartTime)
            {
                var timeJump = newInteraccion.startTime - toReport.endTime;
                var frameJump = newInteraccion.startFrame - toReport.endFrame;

                if(timeJump > 0)
                    toReport.startTime = toReport.startTime + timeJump;
                if(frameJump > 0)
                    toReport.startFrame = toReport.startFrame + frameJump;
            }

            toReport.endTime = Mathf.Max(newInteraccion.endTime, toReport.endTime);
            toReport.endFrame = Mathf.Max(newInteraccion.endFrame, toReport.endFrame);

            toReport.emotionAtMaxValueTimes = toReport.emotionAtMaxValue ? Mathf.Clamp(toReport.emotionAtMaxValueTimes + newInteraccion.emotionAtMaxValueTimes, 1, int.MaxValue) : 0;
            toReport.triggerMaxValueTimes = toReport.triggerMaxValue ? Mathf.Clamp(toReport.triggerMaxValueTimes + newInteraccion.triggerMaxValueTimes, 1, int.MaxValue) : 0;

            toReport.damagePercentageDone += newInteraccion.damagePercentageDone;

            toReport.overshootOrUndershootTotal += newInteraccion.overshootOrUndershoot;

            toReport.damageScoreTotal += newInteraccion.damageScore;
        }
        public static void UnStack(ref Interaction toReport, ref Interaction oldInteraccion, bool removeTimes)
        {


            if(removeTimes)
                toReport.times = toReport.times - 1;


            toReport.stacks = toReport.stacks - oldInteraccion.stacks;

            //there is no need to cahnge time, UnStack its only used on happening interactions, happening interactions will always use max values as end values
            //toReport.endTime = Mathf.Clamp(toReport.endTime - oldInteraccion.duration, toReport.startTime, toReport.endTime);
            //toReport.endFrame = Mathf.Clamp(toReport.endFrame - oldInteraccion.frames, toReport.startFrame, toReport.endFrame);


            toReport.emotionAtMaxValueTimes = toReport.emotionAtMaxValue ? Mathf.Clamp(toReport.emotionAtMaxValueTimes - oldInteraccion.emotionAtMaxValueTimes, 1, int.MaxValue) : 0;
            toReport.triggerMaxValueTimes = toReport.triggerMaxValue ? Mathf.Clamp(toReport.triggerMaxValueTimes - oldInteraccion.triggerMaxValueTimes, 1, int.MaxValue) : 0;

            toReport.damagePercentageDone -= oldInteraccion.damagePercentageDone;

            toReport.overshootOrUndershootTotal -= oldInteraccion.overshootOrUndershoot;

            toReport.damageScoreTotal -= oldInteraccion.damageScore;


        }

        public (TriggeringBodyPart, SensitiveBodyPart, InterationReceivedType, Emotion, bool) GetKey()
        {
            return (fromPart, toPart, interationReceivedType, emotion, triggerMaxValue);
        }

        /// <summary>
        /// how many times the stimulus amount is surpassed or how many times the stimulus amount is insufficient, zero if no damage was done
        /// <para>Warning: For combined or archived interactions, this value is not accurate.</para>
        /// <para>EX: This value is one if the caress's force falls within the appropriate range.</para>
        /// <para>EX: When performing a caress with excessive force, the value will exceed one, indicating the number of times the force exceeds the appropriate amount.</para>
        /// <para>EX: When performing a caress that is too soft below the appropriate range, this value will be less than one, indicating the fraction of the range that was reached below the appropriate range.</para>
        /// <para>This same logic applies to other types of interactions, such as penetration. This refers to the overshoot or undershoot of the length or width of the member inside a hole.</para>
        /// </summary>
        public float overshootOrUndershoot => stacks <= 0 ? 0f : overshootOrUndershootTotal / (float)stacks;

        /// <summary>
        /// zero to one value, It is the quality of the damage done; for example, if there were many "critical hits," then the value is closer to one, and if there were many "grazes," then the value is close to zero.
        /// </summary>
        public float damageScore => stacks <= 0 ? 0f : damageScoreTotal / (float)stacks;
        public float scoredDamagePercentageDone => damagePercentageDone * damageScore * 2f;
        public float GetScoredDamagePercentageDone(float mod) => damagePercentageDone * damageScore * mod;



        /// <summary>
        /// How much damage would this interaction cause if it lasted a second?
        /// </summary>
        public float damagePercentagePerSecond => duration == 0 ? 0 : damagePercentageDone / duration;

        /// <summary>
        /// This is true as long as the emotion is at its maximum value; if this interaction is archived, then it is true if at any time the emotion reached its maximum.
        /// </summary>
        public bool emotionAtMaxValue => emotionAtMaxValueTimes > 0;

        /// <summary>
        /// This is only true if the interaction causes the emotion to reach its maximum value; Then this value is true if any of these archived or combined interactions caused the emotion to reach its maximum.
        /// <para>EX: To find out how many orgasms the "to" character receives from the "from" character, find every pleasure interaction with "reachedMaxValue" set to true. (This is the least efficient way, just an example.)</para>
        /// <para>A pleasure interaction is just an interaction with emotion field set to pleasure.</para>
        /// </summary>
        public bool triggerMaxValue => triggerMaxValueTimes > 0;
        /// <summary>
        /// The duration of the interaction
        /// </summary>
        public float duration => endTime - startTime;
        public int frames => endFrame - startFrame;

        public bool isValid => toID != null && toPart != SensitiveBodyPart.None && fromPart != TriggeringBodyPart.None && interationReceivedType != InterationReceivedType.None && emotion != Emotion.None && times > 0 && stacks > 0;


        /// <summary>
        /// The in-game date on which the scene occurs. If this is an archived past interaction then this value is the first time it happened
        /// </summary>
        public DateTime date
        {
            get
            {
                if(!m_date.HasValue)
                {
                    if(string.IsNullOrWhiteSpace(dateString))
                        m_date = DateTime.MinValue;
                    else
                        m_date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                }
                return m_date.Value;
            }
            set
            {
                m_date = value;
                dateString = m_date.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        [NonSerialized]
        DateTime? m_date;


        public string fromID;
        public string toID;

        public TriggeringBodyPart fromPart;
        public SensitiveBodyPart toPart;
        public InterationReceivedType interationReceivedType;
        public Emotion emotion;


        /// <summary>
        /// The in-game date on which the scene occurs. If this is an archived past interaction then this value is the first time it happened
        /// </summary>
        public string dateString;
        /// <summary>
        /// The time in seconds since the beginning of the scene when the interaction occurs
        /// </summary>
        public float startTime;
        /// <summary>
        /// The time in seconds since the beginning of the scene when the interaction ends; if it is an archived interaction, this value represents the last time it happened.
        /// </summary>
        public float endTime;

        /// <summary>
        /// The frame of the scene when the interaction occurs
        /// </summary>
        public int startFrame;
        /// <summary>
        /// The frame of the scene when the interaction ends; if it is an archived interaction, this value represents the last frame it happened.
        /// </summary>
        public int endFrame;

        /// <summary>
        /// If the interaction is currently taking place, IS ALWAYS 1. If this is an archived past interaction, this value represents the number of times it occurred in the scene.
        /// </summary>
        public int times;

        public int stacks;

        /// <summary>
        /// This interaction results in a change in the target emotion, which is express here as a percentage.
        /// </summary>
        public float damagePercentageDone;


        /// <summary>
        /// If the interaction is currently taking place, this value indicates how many frames the emotions have been at peak. If this is an archived past interaction, this value represents the number of times the emotion was at max value.
        /// </summary>
        public int emotionAtMaxValueTimes;
        /// <summary>
        /// This value indicates whether this same interaction caused the emotion to reach the maximum value; if it is an archived interaction, this value represents the number of times that the emotion was reached at maximum value.
        /// </summary>
        public int triggerMaxValueTimes;






        public float overshootOrUndershootTotal;

        public float damageScoreTotal;









        public InteractionToDisk ToDiskInter()
        {
            InteractionToDisk r = new InteractionToDisk();
            r.fID = fromID;
            r.tID = toID;
            r.fP = fromPart;
            r.tP = toPart;
            r.typ = interationReceivedType;
            r.emo = emotion;
            r.dS = dateString;
            r.sT = startTime;
            r.eT = endTime;
            r.sF = startFrame;
            r.eF = endFrame;
            r.ts = stacks;
            r.tss = times;
            r.dmg = damagePercentageDone;
            r.emoMxTs = emotionAtMaxValueTimes;
            r.tggMxTs = triggerMaxValueTimes;
            r.off = overshootOrUndershootTotal;
            r.dmgS = damageScoreTotal;
            return r;
        }



    }
}
