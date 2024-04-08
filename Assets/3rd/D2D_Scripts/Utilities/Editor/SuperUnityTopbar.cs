using System;
using D2D;
using D2D.Databases;
using UnityEngine;

namespace D2DD
{
    public class SuperUnityTopbar
    {
        protected static bool BigButton(string text, string tooltip = "", Color c = default)
        {
            var style = new GUIStyle("Command")
            {
                fontSize = 10,
                fixedWidth = 60,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };

            return GUILayout.Button(new GUIContent(text, tooltip), style);
        }
        
        protected static bool ButtonTexture(Texture texture, int size = 20)
        {
            return GUILayout.Button(texture, GUILayout.Width(size * 1.5f), GUILayout.Height(size));
        }
        
        protected static bool SmallButton(string text, string tooltip = "", Color c = default)
        {
            var oldColor = GUI.backgroundColor;
            
            if (c != default)
                GUI.backgroundColor = Color.red;
            
            var style = new GUIStyle("Command")
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
            
            GUI.backgroundColor = oldColor;
            
            return GUILayout.Button(new GUIContent(text, tooltip), style);
        }
        
        protected static bool LetterButton(string text, string tooltip = "", Color c = default)
        {
            var style = new GUIStyle("Command")
            {
                fontSize = 10,
                fixedWidth = 30,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };

            return GUILayout.Button(new GUIContent(text, tooltip), style);
        }
        
        protected static bool TinyButton(string text, string tooltip = "", Color c = default)
        {
            var style = new GUIStyle("Command")
            {
                fontSize = 10,
                fixedWidth = 20,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };

            return GUILayout.Button(new GUIContent(text, tooltip), style);
        }
    }
}