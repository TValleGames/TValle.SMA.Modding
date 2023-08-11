using Assets.TValle.Tools.MeshBindPosesCorrector.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.TValle.Tools.MeshBindPosesCorrector
{
    /// <summary>
    /// Used Only by TValle, i mean, it requires de main female avatar
    /// </summary>
    public class LoadBindPoseIntoMapWindow : EditorWindow
    {
        [MenuItem("TValle/Windows/Load Bind Pose Into Map (DO NOT USE)")]
        static void Init()
        {
            LoadBindPoseIntoMapWindow window = EditorWindow.GetWindow<LoadBindPoseIntoMapWindow>(true, null, false);
            window.Show();
        }



        public GameObject femaleAvatar;
        public SkinnedMeshRenderer mainMesh;
        public MapOfFemaleAvatarBindPoses targetMap;
        void OnGUI()
        {
            Editor editor = Editor.CreateEditor(this);
            editor.DrawDefaultInspector();


            if(Application.isPlaying)
                return;


            if(femaleAvatar != null && targetMap != null && GUILayout.Button("Start"))
            {
                var renderers = femaleAvatar.GetComponentsInChildren<SkinnedMeshRenderer>();
                Dictionary<string, (Transform, Matrix4x4)> data = new Dictionary<string, (Transform, Matrix4x4)>();
                Transform[] mainbones = null;


                for(int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
                {
                    var renderer = renderers[rendererIndex];
                    var bones = renderer.bones;
                    var bindPoses = renderer.sharedMesh.bindposes;
                    for(int i = 0; i < bones.Length; i++)
                    {
                        data.TryAdd(bones[i].name, (bones[i], bindPoses[i]));
                    }
                    if(renderer == mainMesh)
                        mainbones = renderer.bones;
                }

                targetMap.SetData(data, mainbones);
                var fromMap = targetMap.GetData();
                if(fromMap.Count == data.Count)
                {
                    EditorUtility.SetDirty(targetMap);
                    Debug.Log("Data Was Set To Map");
                }
                else
                    Debug.LogError("Data Was NOT Set To Map");
            }
        }
    }
}
