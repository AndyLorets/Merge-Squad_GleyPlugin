using System;
using System.Collections.Generic;
using System.Linq;
using AV.Inspector.Runtime;
using D2D.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace D2D.Gameplay
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : SuperEditor
    {
        public override void OnInspectorGUI()
        {
            BeginSerialization();

            var l = target as Level;
            if (!l.IsValid)
                EditorGUILayout.HelpBox("Scene name does not match signature Level_X", MessageType.Warning);
            else
                ReadOnlyLine("_sceneNumber");

            EndSerialization();
        }
    }
}