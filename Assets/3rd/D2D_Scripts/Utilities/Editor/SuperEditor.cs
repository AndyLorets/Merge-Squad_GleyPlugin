using System;
using AV.Inspector.Runtime;
using D2D.Core;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static AV.Inspector.Runtime.SmartInspector;

namespace D2D.Utilities
{
    public class SuperEditor : Editor
    {
        public void SetVector(string boolName, Vector3 v)
        {
            var p = serializedObject.FindProperty(boolName);
            p.vector3Value = v;
            serializedObject.ApplyModifiedProperties();
        }
        
        public Vector3 GetVector(string boolName)
        {
            var p = serializedObject.FindProperty(boolName);
            return p.vector3Value;
        }
        
        public void FlipBool(string boolName)
        {
            var p = serializedObject.FindProperty(boolName);
            p.boolValue = !p.boolValue;
            serializedObject.ApplyModifiedProperties();
        }
        
        
        
        public bool GetBool(string boolName)
        {
            var p = serializedObject.FindProperty(boolName);
            return p.boolValue;
        }
        
        public void InvertInt(string boolName)
        {
            var p = serializedObject.FindProperty(boolName);
            p.intValue = p.intValue == 1 ? -1 : 1;
            serializedObject.ApplyModifiedProperties();
        }
        
        public void PlusEnum(string enumName, int capacity)
        {
            var playMode = serializedObject.FindProperty(enumName);
                    
            if (playMode.enumValueIndex >= capacity-1)
                playMode.enumValueIndex = 0;
            else
            {
                playMode.enumValueIndex++;
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        #region Begin-Ends

        protected void BeginSerialization()
        {
            serializedObject.Update();
        }

        protected void EndSerialization()
        {
            serializedObject.ApplyModifiedProperties();
        }

        protected void BeginReadOnly()
        {
            EditorGUI.BeginDisabledGroup(true);
        }
        
        protected void ReadOnlyLine(string description, string value)
        {
            BeginReadOnly();
            EditorGUILayout.TextField(description, value);
            EndReadOnly();
        }
        
        protected void ReadOnlyLine(string description)
        {
            BeginReadOnly();
            ShowProperty(description);
            EndReadOnly();
        }

        protected void EndReadOnly()
        {
            EditorGUI.EndDisabledGroup();
        }
        
        protected void BeginHorizontal()
        {
            GUILayout.BeginHorizontal("box");
        }

        protected void EndHorizontal()
        {
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Elements

        protected bool Button(string buttonName)
        {
            return GUILayout.Button(buttonName);
        }
        
        protected bool StateButton(string buttonName, string checkBool)
        {
            var c = GetBool(checkBool) ? Color.green : Color.red;
            GUI.backgroundColor = c;
            var isClicked = GUILayout.Button(buttonName);
            GUI.backgroundColor = Color.white;
            return isClicked;
        }
        
        protected bool Button(Texture texture, int size = 20)
        {
            return GUILayout.Button(texture, GUILayout.Width(size * 1.5f), GUILayout.Height(size));
        }

        protected void Space(float size = 6f)
        {
            EditorGUILayout.Space(size);
        }
        
        
        
        protected void ShowProperty(string propertyName, string text="", int maxWidth = -1)
        {
            var property = serializedObject.FindProperty(propertyName);

            if (text == "")
            {
                text = propertyName.Replace('_', ' ').Trim();
                text = (text[0].ToString().ToUpper() + text.Remove(0, 1));
            }
            
            if (maxWidth != -1)
                EditorGUILayout.PropertyField(property, new GUIContent(text), GUILayout.MaxWidth(maxWidth));
            else
                EditorGUILayout.PropertyField(property, new GUIContent(text));
        }

        #endregion
    }
}