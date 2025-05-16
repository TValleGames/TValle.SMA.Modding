using Assets.TValle.Tools.Runtime.Characters.Atts.Emotions;
using Assets.TValle.Tools.Runtime.Characters.Scenes;
using Assets.TValle.Tools.Runtime.Memory;
using Assets.TValle.Tools.Runtime.Moddding;
using Assets.TValle.Tools.Runtime.Moddding.Clothing.Maps;
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
        public delegate void CharacterChangedHandler(SceneCharacter newOne, SceneCharacter oldOne, ISMAJob sender);




        bool isInit { get; }
        bool isAborted { get; set; }

        /// <summary>
        /// the job must produce the main player, use the ISMAJobsManager to produce it
        /// </summary>
        SceneCharacter mainPlayerCharacter { get; }
        /// <summary>
        /// the job must produce the main non player, use the ISMAJobsManager to produce it
        /// </summary>
        SceneCharacter mainNonPlayerCharacter { get; }

        /// <summary>
        /// The job must call this event if there is a switch of main player.
        /// </summary>
        public event CharacterChangedHandler mainPlayerChanged;
        /// <summary>
        /// The job must call this event if there is a switch of main non player.
        /// </summary>
        public event CharacterChangedHandler mainNonPlayerChanged;


        /// <summary>
        /// The in-game date on which the scene occurs.
        /// </summary>
        DateTime date { get; }



        /// <summary>
        /// get this id form the map
        /// </summary>
        string ID { get; }
        /// <summary>
        /// get this name form the map
        /// </summary>
        string Name { get; }
        int lvl { get; }
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
        /// <param name="lvl">starts from zero</param>
        void Init(ISMAJobsManager jobManager, SMAJobMap map, int Lvl, Guid mainPlayerCharacterID, Guid mainNonPlayerCharacterID, DateTime inGameDate);

        /// <summary>
        /// load scenes/assets here, the manager moves this game object to the main job scene once this function returns true.
        /// </summary>
        /// <returns> yield break to move on</returns>
        IEnumerator Load();

        /// <summary>
        /// start the game-logic here, load Objectives
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
        /// Save modifications to characters to the memory and potentially display this information to the player, check Objectives, add experience
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
        bool IsUnlocked(ISMAJobsManager manager, SMAJobMap map);
    }




    public interface ISMAJobsManager
    {
        ISceneInteractions interactions { get; }
        ISMAJobsUIManager UI { get; }
        ISMAJobsObjectives objectives { get; }
        ISMAJobsOutfits outfits { get; }

        /// <summary>
        /// the language selected by the player, if the game supports multiple languages.
        /// </summary>
        Language gameLanguage { get; }




        /// <summary>
        /// job being played
        /// </summary>
        ISMAJob current { get; }

        ///// <summary>
        ///// the male character
        ///// </summary>
        //SceneCharacter mainPlayerCharacter { get; }
        ///// <summary>
        ///// the female character
        ///// </summary>
        //SceneCharacter mainNonPlayerCharacter { get; }

        /// <summary>
        /// data related to a specific job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns>the memory of the job</returns>
        IContextMemory GetMemory(ISMAJob job);
        /// <summary>
        /// data related to a specific job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns>the memory of the job</returns>
        IContextMemory GetMemory(string jobID);


        /// <summary>
        /// data related to a character's job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="character"></param>
        /// <returns>the memory of the character in the job context</returns>
        IContextMemory GetCharacterInMemory(ISMAJob job, SceneCharacter character);
        /// <summary>
        /// data related to a character's job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="character"></param>
        /// <returns>the memory of the character in the job context</returns>
        IContextMemory GetCharacterInMemory(string jobID, string characterID);



        /// <summary>
        /// Scene objects may need some Extra/Default game logic.
        /// </summary>
        /// <param name="scene"></param>
        void AddAdditinalLogicToScene(Scene scene, float phoneAndCameraScreenEmissionModifier);



        /// <summary>
        /// load a character from memory, The game only supports a single character. ALWAYS LOAD THE MALE CHARACTER FIRST
        /// </summary>
        /// <param name="id">empty to load random male character</param>
        /// <param name="feetPosition"></param>
        /// <param name="bodyForwardDirection"></param>
        IEnumerator LoadMaleCharacter(Guid id, Vector3 feetPosition, Vector3 bodyForwardDirection, Action<SceneCharacter> result);

        /// <summary>
        /// load a character from memory, The game only supports a single character.
        /// </summary>
        /// <param name="id">empty to load random female character</param>
        /// <param name="feetPosition"></param>
        /// <param name="bodyForwardDirection"></param>
        IEnumerator LoadFemaleCharacter(Guid id, Vector3 feetPosition, Vector3 bodyForwardDirection, Action<SceneCharacter> result);

        /// <summary>
        /// destroy the character in the scene
        /// </summary>
        /// <param name="id"></param>
        void DestroyCharacter(Guid id);
        /// <summary>
        /// The character in the scene is destroyed and completely erased from memory.
        /// </summary>
        /// <param name="id"></param>
        void DeleteAndDestroyCharacter(Guid id);



        /// <summary>
        /// turn on/off player movement and commands
        /// </summary>
        /// <param name="value"></param>
        void SetMainPlayerCharacterInputsActive(bool value);


        void StartJob(string id, int Lvl, Guid male, Guid female, Action<Exception> OnStaredJobRutine = null);
        void EndCurrentJob(Action<Exception> OnEndedJobRutine = null);
        void AbortCurrentJob(Action<Exception> OnAbortedJobRutine = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="levels">how many lvl this character will increase, Generally this value is less than one, since several sessions are needed to go up a level</param>
        float AddExpToMainPlayerInCurrentJob(float levels);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="levels">how many lvl this character will increase, Generally this value is less than one, since several sessions are needed to go up a level</param>
        float AddExpToMainNonPlayerInCurrentJob(float levels);
        float AddModelingExpToMainNonPlayer( float levels);

        float AddFatigueToCurrentJob(float percentage);
        float AddFatigueToMainNonPlayer(float percentage);
        float PayMoneyToManager(float paymnentAmountModifier);

        float GetExpToMainPlayerInCurrentJob();
        float GetExpToMainNonPlayerInCurrentJob();

    }
    public interface ISMAJobsOutfits
    {

        int CountOfClothingPiecesCoveringBodyPartOfMainPlayer(SensitiveBodyPart bodyPart);

        int CountOfClothingPiecesCoveringBodyPartOfMainNonPlayer(SensitiveBodyPart bodyPart);


        ClothingItemMap.Type GetFirstOrDefaultClothingPiecesCoveringTypeOfMainNonPlayer(SensitiveBodyPart bodyPart);
    }
    public interface ISMAJobsObjectives
    {



        /// <summary>
        ///   create a new ISMAJobObjective instance, this instance need to be added later to the in game job objectives
        /// </summary>
        /// <param name="ID">Global UNIQUE</param>
        /// <param name="description"></param>
        /// <param name="checkAfterCompleted">can go back from Completed To Incompleted</param>
        /// <param name="checkDelegate">returns true for objective completed, returns false for inclompleted</param>
        /// <param name="checkFrequency"> checkDelegate will be called each frame, or can be called every 2 seconds</param>
        /// <param name="subObjectives">(optional)for this objective to be complete, this subObjectives must be completed too</param>
        /// <param name="tips">(optional) give more information to the player</param>
        /// <returns></returns>
        ISMAJobObjective CreateObjective(string ID, string description, bool checkAfterCompleted, ObjectiveCheckerHandler checkDelegate, ObjectiveCheckFrequency checkFrequency, IReadOnlyList<ISMAJobObjective> subObjectives = null, string tips = null);


        /// <summary>
        /// create a new ISMAJobObjective instance, this instance need to be added later to the in game job objectives
        /// <para>EX: The player needs to fill a container, the goal is to fill the container completely to 100%</para>
        /// </summary>
        /// <param name="ID">Global UNIQUE</param>
        /// <param name="description"></param>
        /// <param name="checkAfterCompleted">can go back from Completed To Incompleted</param>
        /// <param name="checkDelegate">returns 1 for objective complete, returns zero for no progress done</param>
        /// <param name="checkFrequency"> checkDelegate will be called each frame, or can be called every 2 seconds</param>
        /// <param name="subObjectives">(optional)for this objective to be complete, this subObjectives must be completed too</param>
        /// <param name="tips">(optional) give more information to the player</param>
        /// <param name="callback">(optional) if the objective Progress changes, this delegate will be called</param>
        /// <returns></returns>
        ISMAJobObjective CreatePercentageObjective(string ID, string description, bool checkAfterCompleted, ObjectiveCheckerHandler_RecalculateWeight checkDelegate, ObjectiveCheckFrequency checkFrequency, IReadOnlyList<ISMAJobObjective> subObjectives = null, string tips = null, PercentageObjectiveProgressWeightChandedHandler callback = null);

        /// <summary>
        /// create a new ISMAJobObjective instance, this instance need to be added later to the in game job objectives
        /// <para>EX: The player needs to collect unique items with specific IDs in his inventory, each ID of each item would be a flag.</para>
        /// </summary>
        /// <param name="ID">Global UNIQUE</param>
        /// <param name="description"></param>
        /// <param name="checkAfterCompleted">can go back from Completed To Incompleted</param>
        /// <param name="Flags">unique ids</param>
        /// <param name="checkDelegate">check if this unique ids flags is set Ex: item with id(flag) is in the inventory. receives the flag(id) returns if is set or not </param>
        /// <param name="checkFrequency"> checkDelegate will be called each frame, or can be called every 2 seconds</param>
        /// <param name="subObjectives">(optional)for this objective to be complete, this subObjectives must be completed too</param>
        /// <param name="tips">(optional) give more information to the player</param>
        /// <param name="callback">(optional) each time a flag is changed, this delegate will be called</param>
        /// <returns></returns>
        ISMAJobObjective CreateFlagsObjective(string ID, string description, bool checkAfterCompleted, IReadOnlyList<string> Flags, ObjectiveCheckerHandler_IsFlagSet checkDelegate, ObjectiveCheckFrequency checkFrequency, IReadOnlyList<ISMAJobObjective> subObjectives = null, string tips = null, ObjectiveFlagsChandedHandler callback = null);


        /// <summary>
        /// create a new ISMAJobObjective instance, this instance need to be added later to the in game job objectives
        /// <para>EX: 'MOVE AROUND' the players need to move arround in all arrow directions, each directions is an unique action.</para>
        /// </summary>
        /// <param name="ID">Global UNIQUE</param>
        /// <param name="description"></param>
        /// <param name="checkAfterCompleted">can go back from Completed To Incompleted</param>
        /// <param name="Capacity">how many of this actions is requred by this objective to be compelted EX: 4 since there are 4 directions the player can move with the arrow</param>
        /// <param name="checkDelegate">Ex: is the player moved forward then 'FORWARD' is returned, or 'BACKWARDS' or 'LEFT' etc, when 4 of this unique actions are returned, the objective will be completed</param>
        /// <param name="checkFrequency"> checkDelegate will be called each frame, or can be called every 2 seconds</param>
        /// <param name="subObjectives">(optional)for this objective to be complete, this subObjectives must be completed too</param>
        /// <param name="tips">(optional) give more information to the player</param>
        /// <param name="callback">(optional) each time the count changes, this delegate will be called</param>
        /// <returns></returns>
        ISMAJobObjective CreateUniqueActionsCountObjective(string ID, string description, bool checkAfterCompleted, int Capacity, ObjectiveCheckerHandler_GetLastUniqueAction checkDelegate, ObjectiveCheckFrequency checkFrequency, IReadOnlyList<ISMAJobObjective> subObjectives = null, string tips = null, ObjectiveCountChandedHandler callback = null);

        /// <summary>
        /// create a new ISMAJobObjective instance, this instance need to be added later to the in game job objectives
        /// <para>EX: player need to jump 5 times.</para>
        /// </summary>
        /// <param name="ID">Global UNIQUE</param>
        /// <param name="description"></param>
        /// <param name="checkAfterCompleted">can go back from Completed To Incompleted</param>
        /// <param name="Capacity">how many of this actions is requred by this objective to be compelted EX: 5</param>
        /// <param name="checkDelegate">Ex: if the player jumped then returns count++</param>
        /// <param name="checkFrequency"> checkDelegate will be called each frame, or can be called every 2 seconds</param>
        /// <param name="subObjectives">(optional)for this objective to be complete, this subObjectives must be completed too</param>
        /// <param name="tips">(optional) give more information to the player</param>
        /// <param name="callback">(optional) each time the count changes, this delegate will be called</param>
        /// <returns></returns>
        ISMAJobObjective CreateCountOfSingleActionObjective(string ID, string description, bool checkAfterCompleted, int Capacity, ObjectiveCheckerHandler_CurrentCount checkDelegate, ObjectiveCheckFrequency checkFrequency, IReadOnlyList<ISMAJobObjective> subObjectives = null, string tips = null, ObjectiveCountChandedHandler callback = null);


        public bool CheckCompleted();
        public void Status(out int totalRequiredObjectives, out int completedRequiredObjectives, out int totalOptionalObjectives, out int completedOptionalObjectives);


        /// <summary>
        /// add this objective to the in game job objectives
        /// </summary>
        /// <param name="objective"></param> 
        /// <param name="required">false for optional objectives</param>
        void AddObjective(ISMAJobObjective objective, bool required);

        /// <summary>
        /// remove this objective from the in game job objectives
        /// </summary>
        /// <param name="objective"></param>
        void RemoveObjective(ISMAJobObjective objective);

        /// <summary>
        /// add this objectives to the in game job objectives
        /// </summary>
        /// <param name="objective"></param>
        /// <param name="required">false for optional objectives</param>
        void AddObjectives(IReadOnlyList<ISMAJobObjective> objective, bool required);

        /// <summary>
        /// remove this objectives from the in game job objectives
        /// </summary>
        /// <param name="objective"></param>
        void RemoveObjectives(IReadOnlyList<ISMAJobObjective> objective);
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

        bool floatingMainMenuIsShowing {  get; }    

        /// <summary>
        /// Displays a floating menu, with the model instance dictating its contents.
        /// </summary>
        /// <param name="model">what is going to be drawn</param>
        /// <param name="previousModel">If the model instance was switched to a different one, a non-null value is returned, which is useful for finalizing the previous model, such as unsubscribing from events.</param>
        void DrawFloatingMainMenuPanel(object model, out object previousModel, Action onHidden = null);
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

        void ShowCurrentJobSessionObjetives(bool show, bool force);





        /// <summary>
        /// Shows the default UI for new buffs and debuffs generated in this scene, it yields when the player accepts
        /// </summary>
        /// <returns> yield break to move on</returns>        
        IEnumerator ShowDefaultEndSessionPanel(bool aborted, float income, float activityExpGain, float activityExpTotal, float modelFatigueGain, float modelFatigueTotal,
            SceneCharacterFromToBuffAndDebuff BuffAndDebuffOnFrom, SceneCharacterFromToBuffAndDebuff BuffAndDebuffOnTo);



    }


}
