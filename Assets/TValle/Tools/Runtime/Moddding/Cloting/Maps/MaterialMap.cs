using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.TValle.Tools.Runtime.Moddding.Cloting.Maps
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
