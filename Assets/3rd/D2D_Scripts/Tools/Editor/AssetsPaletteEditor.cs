using D2D.Utilities;
using UnityEditor;
using UnityEngine;

namespace D2D.Tools
{
    [CustomEditor(typeof(AssetsPalette))]
    public class AssetsPaletteEditor : Editor
    {
        private static readonly string[] DontIncludeMe = {"m_Script"};
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, DontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}