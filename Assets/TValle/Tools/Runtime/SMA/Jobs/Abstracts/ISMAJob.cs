using Assets.TValle.Tools.Runtime.Characters.Scenes;
using Assets.TValle.Tools.Runtime.Memory;
using Assets.TValle.Tools.Runtime.Moddding;
using Assets.TValle.Tools.Runtime.SMA.Moddding.Jobs.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Assets.TValle.Tools.Runtime.SMA.Jobs
{
    /// <summary>
    /// Why IEnumerator isntead of async? im old school :(
    /// </summary>
    public interface ISMAJob
    {
        public bool isJobInitiated { get; }

        /// <summary>
        /// get this id form the map
        /// </summary>
        string ID { get; }
        /// <summary>
        /// get this name form the map
        /// </summary>
        string jobName { get; }

        /// <summary>
        /// load this scene and others in the load funtion call 
        /// </summary>
        Scene mainJobScene { get; }
        /// <summary>
        /// (Optional) load this scene and others in the load funtion call 
        /// </summary>
        Scene lightingAndGeometricsScene { get; }

        /// <summary>
        /// called just before Load, save references to manager and map
        /// </summary>
        /// <param name="mainPlayerCharacterID">the male character</param>
        /// <param name="mainNonPlayerCharacterID">the female character</param>
        void Init(ISMAJobsManager manager, SMAJobMap map, Guid mainPlayerCharacterID, Guid mainNonPlayerCharacterID);

        /// <summary>
        /// load scenes/assets here, the manager moves this game object to the main job scene once this function returns true.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator LoadJob();

        /// <summary>
        /// start the game-logic here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator StartJob();

        /// <summary>
        /// Load character data from memory and potentially display job guides to the player.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator IntroduceJob();



        void BeforeAnimationsUpdate();
        void AfterAnimationsUpdate();


        /// <summary>
        /// Can get multiple calls within a single frame.
        /// </summary>
        void BeforePhysicsUpdate();
        /// <summary>
        /// Can get multiple calls within a single frame.
        /// </summary>
        void AfterPhysicsUpdate();

        void BeforeAIUpdate();
        void AfterAIUpdate();


        /// <summary>
        /// Save modifications to characters to the memory and potentially display this information to the player.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator ConcludeJob();

        /// <summary>
        /// stop all game-logic and do clean up here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator EndJob();

        /// <summary>
        /// unload the scenes/assets here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator UnLoadJob();
    }


    public interface ISMAUnlockableJob
    {
        /// <summary>
        /// true to show inGame, false to hide for now
        /// </summary>
        bool IsUnlocked(ISMAJobsManager manager);
    }




    public interface ISMAJobsManager
    {
        /// <summary>
        /// the language selected by the player, if the game supports multiple languages.
        /// </summary>
        InGameName.Language gameLanguage { get; }

        /// <summary>
        /// job being played
        /// </summary>
        ISMAJob current { get; }

        /// <summary>
        /// the male character
        /// </summary>
        SceneCharacter mainPlayerCharacter { get; }
        /// <summary>
        /// the female character
        /// </summary>
        SceneCharacter mainNonPlayerCharacter { get; }

        /// <summary>
        /// data related to a specific job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns>the memory of the job</returns>
        IContextMemory GetJobMemory(ISMAJob job);
        /// <summary>
        /// data related to a character's job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="character"></param>
        /// <returns>the memory of the character in the job context</returns>
        IContextMemory GetCharacterInJobMemory(ISMAJob job, SceneCharacter character);



        /// <summary>
        /// Scene objects may need some extra game logic.
        /// </summary>
        /// <param name="scene"></param>
        void AddAdditinalLogicToScene(Scene scene);

        /// <summary>
        /// load a character from memory, The game only supports a single character. ALWAYS LOAD THE MALE CHARACTER FIRST
        /// </summary>
        /// <param name="id">empty to load random male character</param>
        /// <param name="feetPosition"></param>
        /// <param name="bodyForwardDirection"></param>
        IEnumerator LoadTheMaleCharacter(Guid id, Vector3 feetPosition, Vector3 bodyForwardDirection);

        /// <summary>
        /// load a character from memory, The game only supports a single character.
        /// </summary>
        /// <param name="id">empty to load random female character</param>
        /// <param name="feetPosition"></param>
        /// <param name="bodyForwardDirection"></param>
        IEnumerator LoadTheFemaleCharacter(Guid id, Vector3 feetPosition, Vector3 bodyForwardDirection);

        /// <summary>
        /// destroy the character in the scene
        /// </summary>
        /// <param name="id"></param>
        void DestroyMaleCharacter(Guid id);
        /// <summary>
        /// The character in the scene is destroyed and completely erased from memory.
        /// </summary>
        /// <param name="id"></param>
        void DeleteAndDestroyMaleCharacter(Guid id);
       
        /// <summary>
        /// destroy the character in the scene
        /// </summary>
        /// <param name="id"></param>
        void DestroyFemaleCharacter(Guid id);

        /// <summary>
        /// The character in the scene is destroyed and completely erased from memory.
        /// </summary>
        /// <param name="id"></param>
        void DeleteAndDestroyFemaleCharacter(Guid id);


        ISceneInteractions interactions { get; }

    }




}
