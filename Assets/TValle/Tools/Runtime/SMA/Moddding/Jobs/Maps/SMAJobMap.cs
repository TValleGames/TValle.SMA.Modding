using Assets.TValle.Tools.Runtime.Moddding;
using Assets.TValle.Tools.Runtime.SMA.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.TValle.Tools.Runtime.SMA.Moddding.Jobs.Maps
{
    [CreateAssetMenu(fileName = "SMAJobMap", menuName = "TValle/SMA/Modding/Jobs/SMAJobMap")]
    public class SMAJobMap : ModdingMap
    {
        public bool IsSMAJobMapValid()
        {
            return !string.IsNullOrWhiteSpace(mainScene?.AssetGUID) && !string.IsNullOrWhiteSpace(portrait?.AssetGUID) && !string.IsNullOrWhiteSpace(mainLogic) && otherPlayersFromPoolsAmount != 0;
        }


        [Header("--- Job Info -----------------------------------------------------------------------")]

        [Header("Scenes")]

        [Space]
        [Tooltip("This scene is always forced to load; the other references to scenes are only loaded on demand.")]
        public AssetReference mainScene;
        [Space]
        [Tooltip("(Optional) These scenes are set as active scenes right after being loaded, meaning that they are used for lighting. (The first field to the left is for a string ID, so you can get the asset reference (second field to the right) in the logic script.)")]
        public AssetReferenceDictionary lightingAndGeometricsScenes = new AssetReferenceDictionary();
        [Space]
        [Tooltip("(Optional) these scenes are simply loaded additively. (The first field to the left is for a string ID, so you can get the asset reference (second field to the right) in the logic script.)")]
        public AssetReferenceDictionary additionalScenes = new AssetReferenceDictionary();


        [Header("Assets")]
        [Space]
        [Tooltip("An image (16:9) of the scenery for this job. (A 100KB image is enough)")]
        public AssetReferenceTexture portrait;

        [Space]
        [Tooltip("The first field to the left is for a string ID, so you can get the asset reference (second field to the right) in the logic script.")]
        public AssetReferenceDictionary extrasResources = new AssetReferenceDictionary();

        [Header("Logic")]

        public OtherPlayerType otherPlayerType;
        public int otherPlayersFromPoolsAmount = -1;

        [Space]
        [Tooltip("(Optional)You can get this data in-game with the id.")]
        public SMAJobObjectivesTextForLevelMap generalObjectivesTextMap;

        [Space]
        [Tooltip("(Index Zero is the initial or default level.) Job-related degrees of difficulty: for instance, a photo shoot may involve models wearing casual clothing, underwear, or nothing at all, for a 3-level total.")]
        public List<Level> levels = new List<Level>();

        [Tooltip("Must implement ISMAJob")]
        [AssemblyQualifiedName(implementingClass = typeof(Component), implementingInterface = typeof(ISMAJob))]
        public string mainLogic;
        [Space]
        [Tooltip("(Optional) Must implement ISMAUnlockableJob")]
        [AssemblyQualifiedName(implementingClass = typeof(Component), implementingInterface = typeof(ISMAUnlockableJob))]
        public string IsUnlockedLogic;

        [Serializable]
        public class Level
        {
            [Tooltip("Income Per Session")]
            public float incomePerSession;
            [Tooltip("(Optional)You can get this data in-game with the id.")]
            public SMAJobObjectivesTextForLevelMap objectivesTextMap;

            [Header("Model Requirements")]
            [Tooltip("Only models that accept modeling can be deployed to this level.")]
            public bool requiresModelingCareer;
            [Tooltip("Only models that accept lingerie modeling can be deployed to this level.")]
            public bool requiresLingerieModeling;
            [Tooltip("Only models that accept erotic modeling can be deployed to this level.")]
            public bool requiresEroticModeling;


            [Header("Text")]
            [Tooltip("How this level will be described in the game in the UI.")]
            public List<InGameNameDesc> inGameDescription;

        }














        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">level is the same as index</param>
        /// <returns></returns>
        public InGameNameDesc GetLevelDesc(int level, Language language)
        {
            if(level < levels.Count)
            {
                var r = levels[level].inGameDescription.FirstOrDefault(d => d.language == language);
                if(r != null)
                    return r;
                return levels[level].inGameDescription.FirstOrDefault(d => d.language == Language.None);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">level is the same as index</param>
        /// <returns></returns>
        public InGameObjectiveText GetObjectiveText(int level, string objectiveID, Language language)
        {
            if(level < levels.Count)
            {
                var levelText = levels[level];
                if(levelText.objectivesTextMap == null)
                    return null;

                var text = levelText.objectivesTextMap.text.FirstOrDefault(ob => ob.id == objectiveID);
                if(text == null)
                    return null;
                var r = text.inGameDescription.FirstOrDefault(tx => tx.language == language);
                if(r != null)
                    return r;
                return text.inGameDescription.FirstOrDefault(tx => tx.language == Language.None);
            }
            return null;
        }
        public InGameObjectiveText GetObjectiveText(string objectiveID, Language language)
        {
            InGameObjectiveText r = null;
            if(generalObjectivesTextMap != null)
            {
                var text = generalObjectivesTextMap.text.FirstOrDefault(ob => ob.id == objectiveID);
                if(text != null)
                {
                    r = text.inGameDescription.FirstOrDefault(tx => tx.language == language);
                    if(r == null)
                        r = text.inGameDescription.FirstOrDefault(tx => tx.language == Language.None);

                    if(r != null)
                        return r;
                }
            }

            for(int i = 0; i < levels.Count; i++)
            {
                r = GetObjectiveText(i, objectiveID, language);
                if(r != null)
                    return r;
            }


            return null;
        }


        public enum OtherPlayerType
        {
            selfAdmin,//the job will decide by it seld who is the male player
            employer_FromPool,
            client_FromPool,
            stranger_FromPool,
        }
    }
}
