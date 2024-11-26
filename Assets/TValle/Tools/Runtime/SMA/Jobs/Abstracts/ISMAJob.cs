using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
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
        public bool isInit { get; }

        /// <summary>
        /// get this id form the map
        /// </summary>
        string ID { get; }
        /// <summary>
        /// get this name form the map
        /// </summary>
        string Name { get; }

        /// <summary>
        /// load this scene and others in the load funtion call 
        /// </summary>
        Scene mainScene { get; }
        /// <summary>
        /// (Optional) load this scene and others in the load funtion call 
        /// </summary>
        Scene lightingAndGeometricsScene { get; }

        /// <summary>
        /// called just before Load, save references to manager and map
        /// </summary>
        /// <param name="mainPlayerCharacterID">the male character</param>
        /// <param name="mainNonPlayerCharacterID">the female character</param>
        void Init(ISMAJobsManager jobManager, SMAJobMap map, Guid mainPlayerCharacterID, Guid mainNonPlayerCharacterID);

        /// <summary>
        /// load scenes/assets here, the manager moves this game object to the main job scene once this function returns true.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator Load();

        /// <summary>
        /// start the game-logic here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator DoStart();

        /// <summary>
        /// Load character data from memory and potentially display job guides to the player.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator Introduce();


        /// <summary>
        /// called once each time an emotion reaches max value, non-al type of character have all emotion types
        /// </summary>
        /// <param name="emotion"></param>
        void OnNonPlayerMaxEmotionValue(Emotion emotion);
        /// <summary>
        /// When an emotion reaches max value, the game will wait for awhile. Here you can control how much the game must wait, only called on max: pleasure, deception/boredom, rage, pain, fear
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns>True to break, false to call this function once again next frame.</returns>
        bool OnNonPlayerMaxEmotionValueBuffer(Emotion emotion);

        /// <summary>
        /// called once each time an emotion reaches max value, non-al type of character have all emotion types
        /// </summary>
        /// <param name="emotion"></param>
        void OnPlayerMaxEmotionValue(Emotion emotion);
        /// <summary>
        /// When an emotion reaches max value, the game will wait for awhile. Here you can control how much the game must wait, only called on max: pleasure
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns>True to break, false to call this function once again next frame.</returns>
        bool OnPlayerMaxEmotionValueBuffer(Emotion emotion);




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
        IEnumerator Conclude();

        /// <summary>
        /// stop all game-logic and do clean up here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator End();

        /// <summary>
        /// unload the scenes/assets here
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator UnLoad();
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
        ISMAJobsUIManager UI { get; }
        /// <summary>
        /// the language selected by the player, if the game supports multiple languages.
        /// </summary>
        Language gameLanguage { get; }

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
        IContextMemory GetMemory(ISMAJob job);
        /// <summary>
        /// data related to a character's job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="character"></param>
        /// <returns>the memory of the character in the job context</returns>
        IContextMemory GetCharacterInMemory(ISMAJob job, SceneCharacter character);



        /// <summary>
        /// Scene objects may need some Extra/Default game logic.
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

        /// <summary>
        /// turn on/off player movement and commands
        /// </summary>
        /// <param name="value"></param>
        void SetMainPlayerCharacterInputsActive(bool value);

        ISceneInteractions interactions { get; }

    }

    public interface ISMAJobsUIManager
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="actionMemberName"></param>
        ///// <param name="actionMemberType"></param>
        ///// <param name="model"></param>
        ///// <param name="volatilMemory">wont be saved to disk</param>
        //public delegate void OnModelAction(string actionMemberName, Type actionMemberType, object model, IContextMemory volatilMemory);


        /// <summary>
        /// When the player releases the key assigned to the action of showing Menu, by default it is tab
        /// </summary>
        event Action<ISMAJobsUIManager> showMenuKeyReleased;



        /// <summary>
        /// Displays a floating menu, with the model instance dictating its contents.
        /// </summary>
        /// <param name="model">what is going to be drawn</param>
        /// <param name="previousModel">If the model instance was switched to a different one, a non-null value is returned, which is useful for finalizing the previous model, such as unsubscribing from events.</param>
        void DrawFloatingMainMenuPanel(object model, out object previousModel);
        /// <summary>
        /// Displays a menu on the main canvas, with the model instance dictating its contents.
        /// </summary>
        /// <param name="model">what is going to be drawn</param>
        /// <param name="previousModel">If the model instance was switched to a different one, a non-null value is returned, which is useful for finalizing the previous model, such as unsubscribing from events.</param>
        void DrawMainMenuPanelOnMainCanvas(object model, out object previousModel);

        void CloseFloatingPanel();
        void CloseMainCanvasPanel();

        /// <summary>
        /// Typically, it shows details about the character the player is currently controlling.
        /// </summary>
        void ShowMainPlayerCharacterInfo();
        /// <summary>
        /// It usually shows the information of the current model present in the scene.
        /// </summary>
        void ShowMainNonPlayerCharacterInfo();
       
        void ShowCurrentJobSessionObjetives();

    }


}
