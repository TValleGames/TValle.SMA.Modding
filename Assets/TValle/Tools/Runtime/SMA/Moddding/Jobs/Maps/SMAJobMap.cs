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
        [Tooltip("The first field to the left is for a string ID, so you can get the asset reference (second field to the right) in the logic script.")]
        public AssetReferenceDictionary extrasResources = new AssetReferenceDictionary();

        [Header("Logic")]
        [Space]

        [Tooltip("Must implement ISMAJob")]
        [AssemblyQualifiedName(implementingClass = typeof(Component), implementingInterface = typeof(ISMAJob))]
        public string mainLogic;
        [Space]
        [Tooltip("(Optional) Must implement ISMAUnlockableJob")]
        [AssemblyQualifiedName(implementingClass = typeof(Component), implementingInterface = typeof(ISMAUnlockableJob))]
        public string IsUnlockedLogic;

        
    }
}
