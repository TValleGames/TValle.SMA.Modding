using Assets.TValle.Tools.Runtime.Moddding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.TValle.Tools.Moddding
{
    public class ClotingModsExportWindows : EditorWindow
    {
        public const string defaultMsgPath = "Auto Generated On Export Mods";


        [MenuItem("TValle/Modding/Windows/Cloting Mods Export Windows")]
        static void Init()
        {
            ClotingModsExportWindows window = EditorWindow.GetWindow<ClotingModsExportWindows>(true, null, false);
            window.UpdateModsList();
            window.Show();
        }


        List<Gruop> m_mods;
        ReorderableList m_modsDrawer;





        void OnGUI()
        {
            if(Application.isPlaying)
                return;

            try
            {
                if(m_modsDrawer == null)
                {
                    EditorGUILayout.LabelField("Press Update mod List");
                    return;
                }

                EditorGUILayout.LabelField("Check ONLY your Cloting Mods To export.");
                m_modsDrawer.DoLayoutList();
                EditorGUILayout.LabelField("", new GUIStyle("CN EntryWarnIcon"), GUILayout.Height(30));
                EditorGUILayout.LabelField("Warning: You can't change the name of the bundle file or the name of the folder it's in. " +
                    "If these files are in a different place, the game won't be able to read them.", EditorStyles.wordWrappedLabel/*,*/ /*new GUIStyle("WarningOverlay"),*/ /*GUILayout.Height(100)*/);

                if(GUILayout.Button("Export"))
                {
                    var settings = AddressableAssetSettingsDefaultObject.Settings;
                    var activeProfileId = settings.activeProfileId;
                    foreach(var mod in m_mods)
                    {
                        if(mod?.settings == null)
                            continue;
                        if(!mod.doExport)
                            continue;

                        var modDir = Directorys.RemoveInvalid(mod.name);


                        settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalBuildPath, Path.Combine(Directorys.clothingModsPath, modDir));
                        settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalLoadPath, Path.Combine(Directorys.clothingModsTypePath, modDir));



                        SetAllGruops(false);
                        var groupSchema = mod.settings.GetSchema<BundledAssetGroupSchema>();
                        groupSchema.IncludeInBuild = true;
                        EditorUtility.SetDirty(mod.settings);
                        AddressableAssetSettings.BuildPlayerContent();
                    }


                    settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalBuildPath, defaultMsgPath);
                    settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalLoadPath, defaultMsgPath);
                }
            }
            finally
            {
                if(GUILayout.Button("Update Mods List"))
                {
                    UpdateModsList();
                }
            }

        }
        void SetAllGruops(bool include)
        {
            foreach(var mod in m_mods)
            {
                if(mod?.settings == null)
                    return;
                var groupSchema = mod.settings.GetSchema<BundledAssetGroupSchema>();
                groupSchema.IncludeInBuild = include;
                EditorUtility.SetDirty(mod.settings);
            }
        }
        void UpdateModsList()
        {
            m_mods = new List<Gruop>();

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach(var sg in settings.groups)
            {
                if(sg.ReadOnly || sg.entries.Count == 0)
                    continue;
                m_mods.Add(new Gruop() { doExport = false, name = sg.name, settings = sg });
            }



            m_modsDrawer = new ReorderableList(m_mods, typeof(Gruop), false, false, false, false);
            m_modsDrawer.elementHeight = EditorGUIUtility.singleLineHeight;
            m_modsDrawer.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = m_mods[index];
                Rect RectToggle = rect;
                RectToggle.width = 25;
                element.doExport = EditorGUI.ToggleLeft(RectToggle, "|", element.doExport, EditorStyles.miniLabel);
                Rect RectLabel = rect;
                RectLabel.x += RectToggle.width;
                RectLabel.width -= RectToggle.width;
                EditorGUI.LabelField(RectLabel, element.name, EditorStyles.boldLabel);

                //Rect RectLabel = rect;
                //RectLabel.height = EditorGUIUtility.singleLineHeight;
                //GUIContent LabelContent = new GUIContent(element.name);
                //EditorGUI.LabelField(RectLabel, LabelContent, EditorStyles.boldLabel);
                //Rect RectToggle = RectLabel;
                //RectToggle.height = EditorGUIUtility.singleLineHeight;
                //RectToggle.x += new GUIStyle(EditorStyles.boldLabel).CalcSize(LabelContent).x + 5;
                //element.doExport = EditorGUI.ToggleLeft(RectToggle, "Check To Export", element.doExport, EditorStyles.miniLabel);
            };
        }




        [Serializable]
        public class Gruop
        {
            //[JustToReadUI]
            public string name;
            public AddressableAssetGroup settings;
            public bool doExport;
        }
    }
}
