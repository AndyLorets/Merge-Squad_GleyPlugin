using System;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using UnityEditor;
using UnityEngine;

namespace D2D.Core
{
    [CustomEditor(typeof(GameStateMachine))]
    public class GameStateMachineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // ...
            
            if (!Application.isPlaying)
                base.OnInspectorGUI();
            
            GameStateMachine gsm = null;
            
            try
            {
                gsm = FindObjectOfType<GameStateMachine>();
            }
            catch (Exception e)
            {
                return;
            }
            
            if (gsm == null)
                return;


            if (!gsm.StatesForEditor.Any())
                return;
            
            var pushedStatesNames = new List<string>();
            gsm.StatesForEditor.ForEach(state => pushedStatesNames.Add(state + ""));

            EditorGUILayout.LabelField("Last state: ");
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField(pushedStatesNames.Last());
            EditorGUI.EndDisabledGroup();
                
            EditorGUILayout.Space();
                
            EditorGUILayout.LabelField("States stack: ");
            
            EditorGUI.BeginDisabledGroup(true);
            var lines = pushedStatesNames;
            int i = 1;
            foreach (var line in lines)
            {
                EditorGUILayout.LabelField($"{i}. {line}");
                i++;
            }
            EditorGUI.EndDisabledGroup();
            // else
            // {
            //     if (!Application.isPlaying)
            //         return;
            //     
            //     EditorGUI.BeginDisabledGroup(true);
            //     EditorGUILayout.LabelField("For now it is empty...");
            //     EditorGUI.EndDisabledGroup();
            // }
        }
    }
}