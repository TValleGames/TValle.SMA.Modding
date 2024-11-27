using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.TValle.Tools.Runtime.Moddding.Clothing.Maps
{
    [CreateAssetMenu(fileName = "MaterialMap", menuName = "TValle/Modding/Cloting/MaterialMap")]
    public class MaterialMap : ModdingMap
    {
        [Space]
        [Header("--- Material Data -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("drop your unity material here. Remember that your material must be in the same group of addressables that corresponds to your mod")]
        public AssetReferenceMaterial materialAddress;

        [Space]
        [Header("--- Info -----------------------------------------------------------------------")]
        [Space]
        public ItemQuality itemQuality = ItemQuality.Epic;

        [Space]
        [Header("--- Outfit Generator Info -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("probability that the generator will choose this item of material. (100% means that it will have the same opportunity as the rest of the materials that also have 100%, 50% means half, and so on).")]
        [Range(0, 100)]
        public float chance = 100;
        [Tooltip("true if you want the generator to make a random color for this material. false if you don't want the color you set in the material to ever change.")]
        public bool canBeCustomColor=true;
        [Tooltip("true if you want the generator to set a random transparency, false if you want your material's transparency to always be the one you set.")]
        public bool canBeCustomOpacity = false;

        [Space]
        [Header("--- Optionals -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("drop your Diffusion Profile here. EXPERIMENTAL: remember that your Diffusion Profile must be in the same group of addressables that corresponds to your mod")]
        public AssetReferenceDiffusionProfiles diffusionProfilesAddress;
    }
    [Serializable]
    sealed public class AssetReferenceDiffusionProfiles : AssetReferenceT<DiffusionProfileSettings>
    {
        public AssetReferenceDiffusionProfiles(string guid) : base(guid)
        {
        }
    }
    [Serializable]
    sealed public class AssetReferenceMaterial : AssetReferenceT<Material>
    {
        public AssetReferenceMaterial(string guid) : base(guid)
        {
        }
    }
}
