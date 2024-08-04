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


        /// <summary>
        /// Get the interactions between two characters currently taking place.
        /// </summary>
        /// <param name="from">for now, always the male character</param>
        /// <param name="to">for now, always the female character</param>
        /// <returns></returns>
        ICharactersSceneInteractions GetTakingPlaceInteractions(SceneCharacter from, SceneCharacter to);

        /// <summary>
        /// Get the interactions between two characters so far.
        /// </summary>
        /// <param name="from">for now, always the male character</param>
        /// <param name="to">for now, always the female character</param>
        /// <returns></returns>
        ICharactersSceneInteractionsClearable GetStackInteractions(SceneCharacter from, SceneCharacter to);


        void EndRecordign();
    }
    public delegate void OnInteractionHandler(ref Interaction newInteraction, ICharactersSceneInteractions sender);
    public delegate void OnInteractionStackHandler(ref Interaction stackedInteraction, ICharactersSceneInteractions sender);
    public delegate void OnCharactersInteractionHandler(ref Interaction newInteraction, ICharactersSceneInteractions Interactions, SceneCharacter from, SceneCharacter to, ISceneInteractions sender);

    /// <summary>
    /// no interaction on the same parts of the same type of the same emotion will be duplicated
    /// <para>EX: It will only record a single caress using the hand on her shoulder giving her pleasure; if there are multiple such events in the scene, it will combine them into one and calculate the total time by adding each individual interaction.</para>
    /// </summary>
    public interface ICharactersSceneInteractions
    {
        /// <summary>
        /// When a new interaction occurs
        /// </summary>
        event OnInteractionHandler onInteraction;
        /// <summary>
        /// Before adding the new interaction into the stacked interaction
        /// </summary>
        event OnInteractionStackHandler onStackingInteraction;
        /// <summary>
        /// After adding the new interaction into the stacked interaction
        /// </summary>
        event OnInteractionStackHandler onInteractionStacked;



        /// <summary>
        /// Get the interactions without emptying the interaction list.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Interaction> Peek();

        /// <summary>
        /// Get the interaction without removing the interaction form the list
        /// </summary>     
        /// <param name="fromPart">The value cannot be equal to "All".</param>
        /// <param name="toPart">The value cannot be equal to "All".</param>
        /// <param name="interationReceivedType">The value cannot be equal to "All".</param>
        /// <param name="emotion">The value cannot be equal to "All".</param>
        /// <param name="reachedMaxValue">This should be true for interactions that cause the emotion to reach its maximum value.</param>
        /// <returns></returns>
        Interaction Peek(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion, bool reachedMaxValue);

        /// <summary>
        /// Get the interactions without removing the interactions form the list
        /// </summary>     
        /// <param name="fromPart">The value can be equal to "All".</param>
        /// <param name="toPart">The value can be equal to "All".</param>
        /// <param name="interationReceivedType">The value can be equal to "All".</param>
        /// <param name="emotion">The value can be equal to "All".</param>
        /// <param name="reachedMaxValue">This should be true for interactions that cause the emotion to reach its maximum value.</param>
        /// <returns></returns>
        IReadOnlyList<Interaction> PeekMany(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion, bool reachedMaxValue);




    }
    /// <summary>
    /// no interaction on the same parts of the same type of the same emotion will be duplicated
    /// <para>EX: It will only record a single caress using the hand on her shoulder giving her pleasure; if there are multiple such events in the scene, it will combine them into one and calculate the total time by adding each individual interaction.</para>
    /// </summary>
    public interface ICharactersSceneInteractionsClearable : ICharactersSceneInteractions
    {

        /// <summary>
        /// Get the interactions and empty the interaction list.
        /// </summary>
        /// <returns></returns>
        IList<Interaction> Get();

        /// <summary>
        /// Get the interaction and remove the interaction form the list
        /// </summary> 
        /// <param name="fromPart">The value cannot be equal to "All".</param>
        /// <param name="toPart">The value cannot be equal to "All".</param>
        /// <param name="interationReceivedType">The value cannot be equal to "All".</param>
        /// <param name="emotion">The value cannot be equal to "All".</param>
        /// <param name="reachedMaxValue">This should be true for interactions that cause the emotion to reach its maximum value.</param>
        /// <returns></returns>
        Interaction Get(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion, bool reachedMaxValue);

        /// <summary>
        /// Get the interactions and remove the interactions form the list
        /// </summary> 
        /// <param name="fromPart">The value can be equal to "All".</param>
        /// <param name="toPart">The value can be equal to "All".</param>
        /// <param name="interationReceivedType">The value can be equal to "All".</param>
        /// <param name="emotion">The value can be equal to "All".</param>
        /// <param name="reachedMaxValue">This should be true for interactions that cause the emotion to reach its maximum value.</param>
        /// <returns></returns>
        IReadOnlyList<Interaction> GetMany(TriggeringBodyPart fromPart, SensitiveBodyPart toPart, InterationReceivedType interationReceivedType, Emotion emotion, bool reachedMaxValue);

        /// <summary>
        /// empty the interaction list
        /// </summary>
        void Clear();
    }
}
