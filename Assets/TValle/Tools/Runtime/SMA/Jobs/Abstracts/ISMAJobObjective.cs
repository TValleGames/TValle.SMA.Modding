using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.SMA.Jobs
{
    public delegate void ObjectiveStatusChandedHandler(ObjectiveStatus newStatus, ObjectiveStatus oldStatus, ISMAJobObjective sender);
    public delegate void ObjectiveProgressChandedHandler(bool completed, ISMAJobObjective sender);
    public delegate void OnObjectiveCompletedHandler(ISMAJobObjective sender);

    public delegate void PercentageObjectiveProgressWeightChandedHandler(float newProgressWeight, float oldProgressWeight, ISMAJobObjective sender);
    public delegate void ObjectiveFlagsChandedHandler(string flag, bool newValue, bool allFlagsSet, ISMAJobObjective sender);
    public delegate void ObjectiveCountChandedHandler(int newValue, int oldValue, ISMAJobObjective sender);


    public delegate bool ObjectiveCheckerHandler();
    public delegate float ObjectiveCheckerHandler_RecalculateWeight();
    public delegate bool ObjectiveCheckerHandler_IsFlagSet(string flag);
    public delegate string ObjectiveCheckerHandler_GetLastUniqueAction();
    public delegate int ObjectiveCheckerHandler_CurrentCount(int capacity, int count);


    public interface ISMAJobObjective
    {
        string id { get; }
        ObjectiveStatus status { get; }
        ObjectiveProgressType progressType { get; }
        ObjectiveCheckFrequency checkFrequency { get; }

        event ObjectiveStatusChandedHandler statusChanged;
        event ObjectiveProgressChandedHandler progressChanged;
        event OnObjectiveCompletedHandler onCompleted;
    }
    public enum ObjectiveCheckFrequency
    {
        /// <summary>
        /// each 2 seconds more or less, good for performance
        /// </summary>
        delayed,
        /// <summary>
        /// every single frame, may impact performance
        /// </summary>
        eachFrame,
    }
    public enum ObjectiveStatus
    {
        inactive,
        active,
        completed,
        failed,
        cleared,
    }
    public enum ObjectiveProgressType
    {
        singleAction,
        flags,
        percentage,
        countOfSingleAction,
        countOfUniqueAction,
    }
}
