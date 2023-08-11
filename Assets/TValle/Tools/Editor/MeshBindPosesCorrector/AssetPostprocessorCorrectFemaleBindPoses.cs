using Assets.TValle.Tools.MeshBindPosesCorrector.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.TValle.Tools.MeshBindPosesCorrector
{
    public class AssetPostprocessorCorrectFemaleBindPoses : AssetPostprocessor
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        //static void BeforeJuegoLanzado()
        //{
        //    m_data = null;
        //}
        public const string label = "CorrectFemaleBindPoses";
        public const string labelChothing = "CorrectFemaleClothesBindPoses";
        public const string labelMap = "FemaleBindPosesMap";
        static IReadOnlyDictionary<string, Matrix4x4> m_data;
        static IReadOnlyDictionary<string, (Vector3, Quaternion)> m_dataPose;
        static IReadOnlyList<string> m_mainBonesNamesData;

        static bool InitMap(GameObject g)
        {
            if(m_data != null && m_data.Count > 0)
                return true;

            string mapaGUID = AssetDatabase.FindAssets("l:" + labelMap).FirstOrDefault();
            if(string.IsNullOrWhiteSpace(mapaGUID))
            {
                Debug.LogError("No map of bind poses was found, making it impossible to make corrections.", g);
                return false;
            }
            var mapPath = AssetDatabase.GUIDToAssetPath(mapaGUID);
            var map = AssetDatabase.LoadAssetAtPath<MapOfFemaleAvatarBindPoses>(mapPath);
            if(map == null)
            {
                Debug.LogError("No map of bind poses was found, making it impossible to make corrections.", g);
                return false;
            }
            return InitBindPoseData(map, g) && InitMainBones(map, g) && InitBindPoseData2(g, m_data);

        }
        static bool InitMainBones(MapOfFemaleAvatarBindPoses map, GameObject g)
        {
            if(m_mainBonesNamesData != null && m_mainBonesNamesData.Count > 0)
                return true;

            m_mainBonesNamesData = map.GetMainBonesData();
            if(m_mainBonesNamesData == null || m_mainBonesNamesData.Count == 0)
            {
                Debug.LogError("No data of main bones was found, making it impossible to make corrections.", g);
                return false;
            }
            return true;
        }
        static bool InitBindPoseData(MapOfFemaleAvatarBindPoses map, GameObject g)
        {
            if(m_data != null && m_data.Count > 0)
                return true;

            m_data = map.GetData();
            if(m_data == null || m_data.Count == 0)
            {
                Debug.LogError("No data of bind poses was found, making it impossible to make corrections.", g);
                return false;
            }
            return true;
        }
        static bool InitBindPoseData2(GameObject g, IReadOnlyDictionary<string, Matrix4x4> data)
        {
            if(m_dataPose != null && m_dataPose.Count > 0)
                return true;

            m_dataPose = data.Select(par => { GetBonePose(par.Value, out Vector3 position, out Quaternion rotation); return (par.Key, position, rotation); })
                .ToDictionary(par => par.Key, par => (par.position, par.rotation));
            if(m_dataPose == null || m_dataPose.Count == 0)
            {
                Debug.LogError("No data of bind poses was found, making it impossible to make corrections.", g);
                return false;
            }
            return true;
        }


        void OnPostprocessModel(GameObject g)
        {
            var lables = AssetDatabase.GetLabels(assetImporter);
            if(lables.Contains(label, StringComparer.Ordinal) || lables.Contains(labelChothing, StringComparer.Ordinal))
            {//fix bind poses
                Debug.Log("****Fixing Bind Poses On: " + g.name);

                if(!InitMap(g))
                    return;

                var renderers = g.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(var item in renderers)
                {
                    FixBindPoses(item);
                }
                Debug.Log("****Finished*****");
            }
            if(lables.Contains(labelChothing, StringComparer.Ordinal))
            {//fix missing bones
                Debug.Log("****Fixing Missing Bones On: " + g.name);

                if(!InitMap(g))
                    return;

                var renderers = g.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(var item in renderers)
                {
                    FixMissingBones(item, g);
                    FixPoses(item);
                }
                Debug.Log("****Finished*****");
            }

        }
        void FixBindPoses(SkinnedMeshRenderer renderer)
        {
            var indexOfBones = renderer.bones.Select((b, i) => (b, i)).ToDictionary(par => par.b.name, par => par.i);
            var mesh = renderer.sharedMesh;
            var bindPoses = mesh.bindposes;
            foreach(var nameIndex in indexOfBones)
            {
                var boneName = nameIndex.Key;
                var boneIndex = nameIndex.Value;
                if(!m_data.TryGetValue(boneName, out Matrix4x4 bindPose))
                    continue;
                bindPoses[boneIndex] = bindPose;
            }
            mesh.bindposes = bindPoses;
            Debug.Log("bind poses were replaced: " + renderer.name);
        }
        void FixMissingBones(SkinnedMeshRenderer renderer, GameObject g)
        {
            var mesh = renderer.sharedMesh;

            Transform missingRoot = CreateChild(g.transform, "MissingBones");
            missingRoot.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.HideInInspector;
            List<Transform> bones = new List<Transform>(renderer.bones);
            HashSet<string> bonesNames = new HashSet<string>(bones.Select(b => b.name));
            List<Matrix4x4> bindPoses = new List<Matrix4x4>(mesh.bindposes);

            for(int i = 0; i < m_mainBonesNamesData.Count; i++)
            {
                var mainBoneName = m_mainBonesNamesData[i];
                if(!bonesNames.Add(mainBoneName))
                    continue;
                var n = CreateChild(missingRoot, mainBoneName);
                n.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.HideInInspector;
                bones.Add(n);
                bindPoses.Add(m_data[mainBoneName]);
            }
            renderer.bones = bones.ToArray();
            mesh.bindposes = bindPoses.ToArray();
            Debug.Log("main bones poses were added: " + renderer.name);
        }
        void FixPoses(SkinnedMeshRenderer renderer)
        {
            var bones = renderer.bones;
            foreach(var bone in bones)
            {
                if(!m_dataPose.TryGetValue(bone.name, out (Vector3, Quaternion) par))
                    continue;
                bone.SetPositionAndRotation(par.Item1, par.Item2);
            }
        }

        static Transform CreateChild(Transform parent, string name)
        {
            Transform @new = new GameObject(name).transform;
            @new.parent = parent;
            @new.localPosition = Vector3.zero;
            @new.localRotation = Quaternion.identity;
            @new.localScale = Vector3.one;
            return @new;
        }
        static void GetBonePose(Matrix4x4 matrix, out Vector3 position, out Quaternion rotation)
        {
            matrix = matrix.inverse;
            position = matrix.MultiplyPoint3x4(Vector3.zero);
            rotation = Quaternion.LookRotation(matrix.MultiplyVector(Vector3.forward), matrix.MultiplyVector(Vector3.up));
        }
    }
}
