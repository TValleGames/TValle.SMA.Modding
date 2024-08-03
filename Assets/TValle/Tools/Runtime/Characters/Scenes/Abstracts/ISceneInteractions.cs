using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Intections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Scenes
{

    public interface ISceneInteractions
    {
        /// <summary>
        /// When a new interaction arises between any characters,
        /// </summary>
        event OnCharactersInteractionHandler onInteraction;


        void StartRecording();


        ///// <summary>
        ///// Get the interactions between two characters so far.
        ///// </summary>
        ///// <param name="from">for now, always the male character</param>
        ///// <param name="to">for now, always the female character</param>
        ///// <returns></returns>
        //ICharactersSceneInteractions GetInteractions(SceneCharacter from, SceneCharacter to);

        /// <summary>
        /// Get the interactions between two characters so far, However, there are no duplicate interactions; just stacked duration time.
        /// </summary>
        /// <param name="from">for now, always the male character</param>
        /// <param name="to">for now, always the female character</param>
        /// <returns></returns>
        ICharactersSceneStackedInteractions GetStackInteractions(SceneCharacter from, SceneCharacter to);


        void EndRecordign();
    }
    public delegate void OnInteractionHandler(ref Interaction newInteraction, ICharactersSceneStackedInteractions sender);
    public delegate void OnInteractionStackHandler(ref Interaction stackedInteraction, ref Interaction newInteraction, ICharactersSceneStackedInteractions sender);
    public delegate void OnCharactersInteractionHandler(ref Interaction newInteraction, ICharactersSceneStackedInteractions Interactions, SceneCharacter from, SceneCharacter to, ISceneInteractions sender);

    public interface ICharactersSceneInteractions
    {
        /// <summary>
        /// When a new interaction occurs
        /// </summary>
        event OnInteractionHandler onInteraction;

        /// <summary>
        /// Get the interactions without emptying the interaction list.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Interaction> Peek();
        /// <summary>
        /// Get the interactions and empty the interaction list.
        /// </summary>
        /// <returns></returns>
        IList<Interaction> Get();

        /// <summary>
        /// Get the interactions without removing the item from the list.
        /// </summary>
        /// <param name="emotion">Only interactions of this emotion will be selected</param>
        /// <returns></returns>
        IReadOnlyList<Interaction> Peek(Emotion emotion);
        /// <summary>
        /// Get the interactions and remove the item from the list.
        /// </summary>
        /// <param name="emotion">Only interactions of this emotion will be selected</param>
        /// <returns></returns>
        IList<Interaction> Get(Emotion emotion);




       
        /// <summary>
        /// empty the interaction list
        /// </summary>
        void Clear();

    }
    /// <summary>
    /// no interaction on the same parts of the same type will be duplicated
    /// <para>EX: It will only record a single caress using the hand on her shoulder; if there are multiple such events in the scene, it will combine them into one and calculate the total time by adding each individual interaction.</para>
    /// </summary>
    public interface ICharactersSceneStackedInteractions : ICharactersSceneInteractions
    {
        /// <summary>
        /// Before adding the new interaction into the stacked interaction
        /// </summary>
        event OnInteractionStackHandler onStackingInteraction;
        /// <summary>
        /// After adding the new interaction into the stacked interaction
        /// </summary>
        event OnInteractionStackHandler onInteractionStacked;


        /// <summary>
        /// Get the interactions without removing the interaction form the list
        /// </summary>
        /// <returns></returns>
        Interaction Peek(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion);
        /// <summary>
        /// Get the interactions and remove the interaction form the list
        /// </summary>
        /// <returns></returns>
        Interaction Get(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion);

    }
}
