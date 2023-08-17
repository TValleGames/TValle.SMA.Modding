using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.TValle.Tools.Runtime.Moddding.Clothing.Maps
{
    [CreateAssetMenu(fileName = "ClothingItemMap", menuName = "TValle/Modding/Cloting/ClothingItemMap")]
    public class ClothingItemMap : ModdingMap
    {
        [Space]
        [Header("--- Clothing Item Data -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("drop a renderer GameObject here. Remember that your clothing item must be in the same group of addressables that corresponds to your mod.")]
        public AssetReferenceGameObject address;
        [Tooltip("how deep this Clothing Item is.")]
        public Layer layer = Layer.clothes;
        [Tooltip("If any of the materials use tessellation, the game will set the distances and amounts. Set false if none of your materials uses tessellation or you want to configure it yourself; set true if you have any material with tessellation and you want the game to configure it.")]
        public bool gameConfigsTessellation;
        [Tooltip("If your mesh is not so complex and you want it to be able to collide against semen, set this to true. (Read/Write import settings MUST be on.)")]
        public bool canCollideAgainstSemen;
        [Tooltip("If your clothing item covers the breasts, it is advisable to select left and right breasts, the same for the buttocks, and the skin around the anus. if covers the breasts or ass, it dramatically improves the visual aspect of this clothing item. Remember that you need to have transfer the vertex-colors from the sample mesh to your clothing mesh.")]
        public NormalRecalculators normalRecalculators;
        [Tooltip("MODIFY ONLY IF this Clothing Item has high heels.")]
        public HeelConfig heelConfig = new HeelConfig();
        [Tooltip("MODIFY ONLY IF this Clothing has a transformative effect on the bust.")]
        public BreastConfig breastConfig = new BreastConfig();
        [Tooltip("MODIFY ONLY IF this Clothing has a transformative effect on the ass.")]
        public AssConfig assConfig = new AssConfig();
        [Tooltip("MODIFY ONLY IF this Clothing has a transformative effect on the vagina.")]
        public VaginaConfig vaginaConfig = new VaginaConfig();
        [Tooltip("MODIFY ONLY IF this Clothing has a transformative effect on the anus.")]
        public AnusConfig anusConfig = new AnusConfig();

        [Space]
        [Header("--- AI Info -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("When asked to undress, where will the character be self-interacting? (TODO: only implemented torso and hips)")]
        public SelfUndressingPosition selfUndressingPosition = SelfUndressingPosition.torso;
        [Tooltip("select all body parts that this clothing item covers. If this clothing item is a subclotting item from an interacion, it needs to exclude at least one body part.")]
        public Covers covers;



        [Space]
        [Header("--- Outfit Generator Info -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("Since the male and female avatars are different, it is important to know which gender this piece of clothing is for when creating random outfits.")]
        public Sex sex;
        [Tooltip("Are they underwear or stockings? either the upper body or lower body? It is important when creating random outfits.")]
        public Type type;
        [Tooltip("probability that the generator will choose this item of clothing. (100% means that it will have the same opportunity as the rest of the clothes that also have 100%, 50% means half, and so on).")]
        [Range(0, 100)]
        public float chance = 100;
        [Tooltip("if this clothing item does not go well with socks, set this to false.")]
        public bool canWearStockings = true;

        [Space(20)]
        [Header("--- Clothing Materils Data-----------------------------------------------------------------------")]
        [Space]
        [Header("If this clothing item is a sub-clothing item,\nthis field can be left empty,\n and this clothing sub-item will inherit\n the materials of the parent clothing item.")]

        [Tooltip("Every single possible material combination must be declared here. If you don't want the game to have control over your materials, leave this empty.")]
        public List<MaterialData> materialsPerIndex;


        [Space]
        [Header("--- Optionals -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("EXPERIMENTAL: Create custom scripts implementing ICustomClothingItemScript and declare them here to be loaded into the game.")]
        public List<CustomScript> customScripts;
        [Tooltip("thow a armature root GameObject. WARNING: Use ONLY if the piece of clothing has a custom armature, such as glasses, a skirt, a tail, etc., whose bones do not exist in the game yet. (You'll have to animate them yourself.")]
        public AssetReferenceGameObject customArmatureAddress;
        [Tooltip("WARNING: Use ONLY if the piece of clothing has a custom colliders, The game will turn these renders into colliders.")]
        public List<CustomCollider> customColliders;

        [Space]
        [Header("--- Optionals: sub Clothing Items -----------------------------------------------------------------------")]
        [Space]
        [Tooltip("If this item of clothing has any sub-items, specify them here.")]
        public List<Interaction> interactionsToSubClothingItems;













        protected override void OnValidate()
        {
            base.OnValidate();


            if(materialsPerIndex != null)
                for(int i = 0; i < materialsPerIndex.Count; i++)
                    materialsPerIndex[i].materialIndex = i;
        }




        [Serializable]
        public class HeelConfig
        {
            [Tooltip("In centimetres.")]
            public float toeHeigth;
            [Tooltip("In centimetres.")]
            public float heelHeigth;
        }


        [Serializable]
        public class BreastConfig
        {
            [Range(1f, 4f)]
            [Tooltip("If this clothing item supports the breasts or pushes them up, increase this value.")]
            public float siffnessModifier = 1;
            [Range(0.25f, 1f)]
            [Tooltip("If this clothing item has a rigid cup, decrease this value.")]
            public float nippleProjectionModifier = 1;

            [Tooltip("If the nipple shape is obscured by this clothing item, leave this value at -1; if the nipple shape is fully visible, leave it at 1.")]
            [Range(-1f, 1f)]
            public float nippleShapeModifier = 1;

            [Tooltip("If this clothing item pushes the breasts together, decrease this value.")]
            [Range(0f, 1f)]
            public float distanceBetweenModifier = 1;
        }
        [Serializable]
        public class AssConfig
        {
            [Range(1f, 4f)]
            [Tooltip("If this clothing item supports the ass or pushes it up, increase this value.")]
            public float siffnessModifier = 1;
        }
        [Serializable]
        public class VaginaConfig
        {
            [Tooltip("If this item of clothing pushes the labia of the vagina inward, increase this value.")]
            [Range(0f, 1f)]
            public float labiaShrinker = 0;
            [Tooltip("If this clothing item squeezes together the labia of the vagina, increase this value.")]
            [Range(0f, 1f)]
            public float shrinker = 0;
            [Tooltip("If this clothing item hides the wear on the vagina, decrease this value.")]
            [Range(0f, 1f)]
            public float wearModifier = 1;
        }
        [Serializable]
        public class AnusConfig
        {
            [Tooltip("If this clothing item hides the wear on the anus, decrease this value.")]
            [Range(0f, 1f)]
            public float wearModifier = 1;
        }

        [Serializable]
        public class Interaction
        {
            [Tooltip("In order to transform this garment into this sub-garment, which interaction animation must be executed?")]
            public InterationsAnimation animation;
            [Tooltip("Drop the sub-garment's map here. Keep in mind that this sub-map and its associated assets must be placed in the same modding package or group.")]
            public ClothingItemMap subClothingItemMap;
            [Tooltip("When the interaction animation is running, which shape in the mesh should be active?")]
            public string toSubClothingItemShapeName;
            [Tooltip("When activating the shape morph on a sub-garment, there may be visible unwanted results; specify here what corrective shapes your mesh has.")]
            public List<CorrectiveShapes> corrections = new List<CorrectiveShapes>();
            [Serializable]
            public class CorrectiveShapes
            {
                [Tooltip("a zero-to-one animation curve, where t is the weight of the zero-to-one animation, meaning that the maximum value of this curve must be one and the time too.")]
                public AnimationCurve inOut1x1Curve;
                [Tooltip("When the correction is running, which shape in the mesh should be active?")]
                public string correctiveShapeName;
            }
        }

        [Flags]
        public enum NormalRecalculators
        {
            None = 0,
            leftBreast = 1,
            rightBreast = 2,
            leftGluteus = 4,
            rightGluteus = 8,
            leftAnusOpening = 16,
            rightAnusOpening = 32,
        }
        public enum InterationsAnimation
        {
            None = 0,

            exposeLegL = 1,
            exposeLegR = 2,

            exposeAssL = 3,
            exposeAssR = 4,

            exposeAssSideL = 5,
            exposeAssSideR = 6,

            exposeVagAnusL = 7,
            exposeVagAnusR = 8,

            exposeNipplesL = 9,
            exposeNipplesR = 0,

            exposeShouldersL = 11,
            exposeShouldersR = 12,


            exposeAssHalf1L = 13,
            exposeAssHalf1R = 14,

            exposeAssHalf2L = 15,
            exposeAssHalf2R = 16,


            pullDownAssHalf1L = 17,
            pullDownAssHalf1R = 18,

            pullDownAssHalf2L = 19,
            pullDownAssHalf2R = 20,

            exposeCrotchF = 21,

            pullDownAssL = 22,
            pullDownAssR = 23,

            exposeTorsoHalf1F = 24,
            exposeTorsoHalf2F = 25,
            exposeTorsoHalf1B = 26,
            exposeTorsoHalf2B = 27,

            exposeChestLateralHalf1L = 28,
            exposeChestLateralHalf1R = 29,

            exposeChestLateralHalf2L = 30,
            exposeChestLateralHalf2R = 31,


            exposeHipsHalf1L = 32,
            exposeHipsHalf1R = 33,
            exposeHipsHalf2L = 34,
            exposeHipsHalf2R = 34,
        }
        public enum Sex
        {
            None = 0,
            male = 1,
            female = 2,
        }
        public enum Type
        {
            None = 0,
            lowerBodyUnderwear = 1,
            upperBodyUnderwear = 2,
            upperBodyUnderwearAccessories = 11,
            lowerBodyUnderwearAccessories = 12,
            lowerBody = 3,
            upperBody = 4,
            shoes = 5,
            jacket = 6,
            swimsuit = 7,
            accessories = 9,
            glasses = 10,
            socks = 8,
            gloves = 13,
            hat = 14,
        }
        public enum Layer
        {
            underUnderwear = 0,
            underwear = 1,
            underClothes = 2,
            clothes = 3,
            underAccessories = 4,
            accessories = 5,
            underCoat = 6,
            coat = 7,
        }
        public enum SelfUndressingPosition
        {
            None = 0,
            torso = 1,
            hips = 2,
            scalp = 3,
            eyes = 4,
            mouth = 5,
            feet = 6,
            hands = 7,
        }
        [Flags]
        public enum Covers
        {
            None = 0,
            head = 16777216,
            face = 1048576,
            eyes = 131072,
            lips = 65536,
            mouthHole = 1024,//to block penetrations
            neck = 4194304,
            shoulders = 262144,
            arms = 256,
            forearms = 512,
            hands = 16384,
            torso = 2097152,
            back = 8388608,
            breast = 1,
            nipples = 2,
            belly = 524288,
            pubicArea = 32768,
            buttocks = 16,
            labia = 4,
            vaginalHole = 8, //to block penetrations
            analHole = 32,//to block penetrations
            penis = 2048,//if male
            testicles = 4096,//if male
            legs = 64,
            calf = 128,
            feet = 8192,
        }
        [Serializable]
        public class MaterialData
        {
            [Tooltip("the same index as the material in your renderer or your mesh.")]
            [JustToReadUI]
            public int materialIndex;

            [Tooltip("Drop the Material Maps that you created here. All materials that can be in this material index.")]
            public List<MaterialMap> materials;
        }

        [Serializable]
        public class CustomScript
        {
            [AssemblyQualifiedName]
            public string assemblyQualifiedName;
        }
        [Serializable]
        public class CustomCollider
        {
            [Tooltip("Drop a renderer GameObject to be converted into a collider by the game here.")]
            public AssetReferenceGameObject address;

            [Tooltip("This collider can be animated, so... will this collider be animated using the avatar bones or your custom armature bones?")]
            public AnimationArmature animationArmature;

            public enum AnimationArmature
            {
                avatar = 0,
                custom = 1,
            }
        }
    }
}
