using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IPrintableBuff
    {
        string DebugPrint();
        string RichPrint(Func<string, string> characterNameGetter, Language language);
        string RichPrintStandAlone(Func<string, string> characterNameGetter, Language language);

        DisplayableBuffCategory category { get; }
    }

    public enum DisplayableBuffCategory
    {
        None,
        [Label("Relationship Growth", Language.en)]
        favorability,
        [Label("Satisfaction Progress", Language.en)]
        pleasure,
        [Label("Drive Development", Language.en)]
        desires,
        [Label("Anger Management Evolution", Language.en)]
        rage,
        [Label("Pain Endurance Building", Language.en)]
        pain,
        [Label("Trust Level Progress", Language.en)]
        fear,
        [Label("Motivation Progress", Language.en)]
        decep,
        [Label("Additional Development", Language.en)]
        other,
    }
   
    public static class IPrintableBuffEXT
    {       
        public static DisplayableBuffCategory ParseToCategory(this Emotion emo)
        {
            switch(emo)
            {

                case Emotion.enjoyment:
                case Emotion.relief:
                case Emotion.pleasure:
                case Emotion.arousal:
                case Emotion.disgust:
                    return DisplayableBuffCategory.pleasure;
                case Emotion.favorability:
                    return DisplayableBuffCategory.favorability;
                case Emotion.disappointment:
                    return DisplayableBuffCategory.decep;
                case Emotion.rage:
                    return DisplayableBuffCategory.rage;
                case Emotion.pain:
                    return DisplayableBuffCategory.pain;
                case Emotion.fear:
                    return DisplayableBuffCategory.fear;
                default:
                    return DisplayableBuffCategory.None;
            }
        }
        public static string GetOperationSymbol(this Operation op, float value)
        {
            switch(op)
            {
                case Operation.None:
                    return "error";
                case Operation.add:
                    return value < 0 ? string.Empty : "+";
                case Operation.mult:
                    return "×";
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
        public static string GetOperationSymbol(this AddOperation op, float value)
        {
            switch(op)
            {
                case AddOperation.None:
                    return "error";
                case AddOperation.add:
                    return value < 0 ? string.Empty : "+";
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
        public static string GetOperationSymbol(this ProductOperation op, float value)
        {
            switch(op)
            {
                case ProductOperation.None:
                    return "error";
                case ProductOperation.mult:
                    return "×";
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
    }
}
