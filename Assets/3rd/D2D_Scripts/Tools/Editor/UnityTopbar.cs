using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D;
using D2D.Databases;
using D2D.Gameplay;
using D2D.Tools;
using D2D.Utilities;
using D2D.Utils;
using D2DD;
using NUnit.Framework;
using UnityEditor;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;
//using UnityToolbarExtender;

namespace D2D
{
    [InitializeOnLoad]
    public class UnityTopbar : SuperUnityTopbar
    {
        private static Transform _selectedTransform;
        
        static UnityTopbar()
        {
            //ToolbarExtender.LeftToolbarGUI.Add(Left);
            //ToolbarExtender.RightToolbarGUI.Add(Right);
        }

        private static void Left()
        {
            if (!_coreData.tools.showLeftTopBar)
                return;
            
            // if (SmallButton("Col", "Collapse folders in project"))
            //     EditorCollapseAll.CollapseFolders();
            
            if (LetterButton("+F", "New folder"))
                CallMenuItem("Assets/Create/Folder");
            
            if (LetterButton("+M", "New material"))
                CallMenuItem("Assets/Create/Material");
            
            /*if (LetterButton("+N", "New note"))
                CallMenuItem("GameObject/StickyNotes/Create note");*/
            
            /*if (SmallButton("DB", "Clear DB"))
                Shortcuts.ClearDB();*/
            
            /*if (LetterButton("Up️", "Navigate to parent. Cmd+'"))
               Shortcuts.SelectParent();*/

               /*if (SmallButton("Fold", "Fold. Cmd+]"))
                   Shortcuts.FoldParent();*/

            if (SmallButton("Scene", "Select scene asset"))
                Shortcuts.SelectScene();
            
            /*if (SmallButton("Keep", "Keep hierarchy. Ctrl+."))
                HierarchyKeeper.Keep();*/
            
            /*if (SmallButton("Mat", "Show material usages in project. Cmd+M"))
                Shortcuts.SelectChildrenMaterial();*/

            /*if (SmallButton("Pack", "Export package"))
            {
                var a = AssetsPalette.Instance;
                if (!a.toExport.IsNullOrEmpty())
                {
                    Selection.objects = a.toExport;
                }
                CallMenuItem("Assets/Export Package...");
            }*/

            if (SmallButton("Build"))
            {
                CoreSettings.Instance.ApplyProductionData();
                CallMenuItem("File/Build Settings...");

                return;

                var pathToScenes = "Assets/Scenes/";
                var pathToStarter = "Other/Starter.unity";
                // var scenesCount = _coreData.loopRangeLevel.y;
                var scenes = new List<string>();
                scenes.Add(pathToScenes + pathToStarter);
                // for (int i = 1; i <= scenesCount; i++)
                   // scenes.Add(pathToScenes + _coreData.levelScenePrefix + i + ".unity");
                scenes.ForEach(s => Debug.Log(s));
                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = scenes.ToArray(),
                    locationPathName = $"{System.IO.Directory.GetCurrentDirectory()}/Builds/{_coreData.production.appName}.apk",
                    target = BuildTarget.Android,
                    options = BuildOptions.None
                };
                BuildPipeline.BuildPlayer(buildPlayerOptions);
                // BuildSummary summary = report.summary;
                // BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
            }
            
            if (BigButton("Copy T"))
            {
                _selectedTransform = Selection.activeTransform;
            }
            
            if (BigButton("Paste T"))
            {
                Selection.activeTransform.position = _selectedTransform.position;
                Selection.activeTransform.rotation = _selectedTransform.rotation;
                Selection.activeTransform.localScale = _selectedTransform.localScale;
            }
        }

        private static void Right()
        {
            GUILayout.FlexibleSpace();

            if (_coreData.tools.showMiddleTopBar)
            {
                /*if (LetterButton("P", "Select player"))
                    Shortcuts.SelectPlayer();
            
                if (LetterButton("L", "Select level"))
                    Shortcuts.SelectLevel();
            
                if (LetterButton("C", "Select canvas"))
                    Shortcuts.SelectCanvas();*/
                
                if (TinyButton("1", "Select all objects with tag 1 script"))
                    Shortcuts.SelectTag1();
            
                if (TinyButton("2", "Select all objects with tag 2 script"))
                    Shortcuts.SelectTag2();
            
                if (TinyButton("3", "Select all objects with tag 3 script"))
                    Shortcuts.SelectTag3();
                
                if (TinyButton("4", "Select all objects with tag 4 script"))
                    Shortcuts.SelectTag4();
                
                GUILayout.Space(15);
            }


            if (_coreData.tools.showRightTopBar)
            {
                /*if (SmallButton("Mesh", "Show mesh usages in project. Cmd+L"))
                    Shortcuts.SelectChildrenMesh();*/

                // GUILayout.Space(10);
                
                if (LetterButton("C", "Core settings. Cmd+I", Color.blue))
                    Shortcuts.SelectCoreSettings();

                if (LetterButton("G", "Game settings. Cmd+O", Color.blue))
                    Shortcuts.SelectGameSettings();

                if (ButtonTexture(_coreData.tools.icons.effects, 20))
                {
                    HierarchyKeeper.Keep();
                    AssetsPaletteWindow.Init();
                }
            }
        }
        
        private static void CallMenuItem(string t) =>
            EditorApplication.ExecuteMenuItem(t);
    }
}