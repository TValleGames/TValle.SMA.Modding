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
        public const string labelMap = "FemaleBindPosesMap";
        static IReadOnlyDictionary<string, Matrix4x4> m_data;
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
            m_data = map.GetData();
            if(m_data == null || m_data.Count == 0)
            {
                Debug.LogError("No data of bind poses was found, making it impossible to make corrections.", g);
                return false;
            }
            return true;
        }


        void OnPostprocessModel(GameObject g)
        {
            var lables = AssetDatabase.GetLabels(assetImporter);
            if(lables.Contains(label, StringComparer.Ordinal))
            {
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
    }
}
