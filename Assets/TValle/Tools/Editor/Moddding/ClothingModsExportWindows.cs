using Assets.TValle.Tools.Runtime.Moddding;
using Assets.TValle.Tools.Runtime.Moddding.Clothing.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.TValle.Tools.Moddding
{
    public class ClothingModsExportWindows : EditorWindow
    {
        public const string defaultMsgPath = "Auto Generated On Export Mods";


        [MenuItem("TValle/Modding/Windows/Clothing Mods Export Windows")]
        static void Init()
        {
            ClothingModsExportWindows window = EditorWindow.GetWindow<ClothingModsExportWindows>(true, "Clothing Mods Export Windows", true);
            window.UpdateModsList();
            var cR = window.position;
            if(cR.width < 400 || cR.height < 520)
            {
                if(cR.width < 400)
                    cR.width = 400;
                if(cR.height < 520)
                    cR.height = 520;
                window.position = cR;
            }
            window.Show();
        }

        Vector2 scrollPosition = Vector2.zero;
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

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(400), GUILayout.Height(200));
                m_modsDrawer.DoLayoutList();
                GUILayout.EndScrollView();

                EditorGUILayout.LabelField("", new GUIStyle("CN EntryWarnIcon"), GUILayout.Height(30));
                EditorGUILayout.LabelField("Remember that when you publish a mod, you have to publish the whole folder, which includes the.bundle and.json files.", EditorStyles.wordWrappedLabel);
                EditorGUILayout.LabelField("The.hash file is not necessary, but if it is there, it will be used to check if the.bundle file is corrupted.", EditorStyles.wordWrappedLabel);
                EditorGUILayout.LabelField("", new GUIStyle("CN EntryWarnIcon"), GUILayout.Height(30));
                EditorGUILayout.LabelField("Warning: You can't change the name of the bundle file or the name of the folder it's in. " +
                    "If these files are in a different place, the game won't be able to read them.", EditorStyles.wordWrappedLabel/*,*/ /*new GUIStyle("WarningOverlay"),*/ /*GUILayout.Height(100)*/);

                EditorGUILayout.LabelField("", new GUIStyle("CN EntryWarnIcon"), GUILayout.Height(30));
                EditorGUILayout.LabelField("Remember If your clothing mods have custom scripts, you must also publish the .dll files that were copied to the modding script folder.", EditorStyles.wordWrappedLabel);


                if(GUILayout.Button("Export"))
                {

                    var settings = AddressableAssetSettingsDefaultObject.Settings;
                    var activeProfileId = settings.activeProfileId;
                    bool scriptsModsExported = false;

                    foreach(var mod in m_mods)
                    {
                        if(mod?.settings == null)
                            continue;
                        if(!mod.doExport)
                            continue;


                        List<AddressableAssetEntry> clothingMapsAddres = new List<AddressableAssetEntry>();
                        mod.settings.GatherAllAssets(clothingMapsAddres, true, true, true, (entry => entry.MainAssetType == typeof(ClothingItemMap)));
                        foreach(var mapAdress in clothingMapsAddres)
                        {
                            var map = mapAdress.MainAsset as ClothingItemMap ?? mapAdress.TargetAsset as ClothingItemMap;
                            //EditorUtility.SetDirty(map);
                            //AssetDatabase.SaveAssets();

                            foreach(var cs in map.customScripts)
                            {
                                try
                                {
                                    if(string.IsNullOrWhiteSpace(cs.assemblyQualifiedName))
                                        continue;
                                    var t = Type.GetType(cs.assemblyQualifiedName);
                                    if(t == null)
                                        continue;
                                    string assemblyPath = t.Assembly.Location;
                                    Directory.CreateDirectory(Directorys.scriptingModsPath);
                                    File.Copy(assemblyPath, Path.Combine(Directorys.scriptingModsPath, Path.GetFileName(assemblyPath)), true);
                                    scriptsModsExported = true;
                                }
                                catch(Exception e)
                                {
                                    Debug.LogException(e);
                                }

                            }



                            //map?.OnExport();

                        }


                        var modDir = Directorys.RemoveInvalid(mod.name);
                        var completeModDir = Path.Combine(Directorys.clothingModsPath, modDir);

                        {
                            Debug.Log("Deleting old files in : " + completeModDir);

                            DirectoryInfo completeModDirDirectoryInfo = new DirectoryInfo(completeModDir);
                            var bundleFiles = completeModDirDirectoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly).Where(f => f.Extension == ".bundle").Select(f => f.FullName);
                            foreach(var item in bundleFiles)
                            {
                                File.Delete(item);
                            }
                        }


                        settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalBuildPath, completeModDir);
                        settings.profileSettings.SetValue(activeProfileId, AddressableAssetSettings.kLocalLoadPath, Path.Combine(Directorys.clothingModsTypePath, modDir));



                        SetAllGruops(false);
                        var groupSchema = mod.settings.GetSchema<BundledAssetGroupSchema>();
                        groupSchema.IncludeInBuild = true;
                        EditorUtility.SetDirty(mod.settings);
                        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

                        if(string.IsNullOrWhiteSpace(result.Error))
                        {
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                Arguments = completeModDir,
                                FileName = "explorer.exe",
                            };

                            System.Diagnostics.Process.Start(startInfo);
                        }
                    }

                    if(scriptsModsExported)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            Arguments = Directorys.scriptingModsPath,
                            FileName = "explorer.exe",
                        };
                        System.Diagnostics.Process.Start(startInfo);
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
