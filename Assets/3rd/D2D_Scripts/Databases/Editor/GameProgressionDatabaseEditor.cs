using D2D.Utilities;
using UnityEditor;
using UnityEngine;

namespace D2D.Databases
{
    [CustomEditor(typeof(GameProgressionDatabase))]
    public class GameProgressionDatabaseEditor : SuperEditor
    {
        public override void OnInspectorGUI()
        {
            var db = (GameProgressionDatabase) target;

            if (Application.isPlaying)
            {
                BeginReadOnly();
                EditorGUILayout.FloatField("Player money", db.Money.Value);
                EditorGUILayout.IntField("Last scene number", db.LastSceneNumber.Value);
                EditorGUILayout.IntField("Passed levels", db.PassedLevels.Value);
                EndReadOnly();
            }
            else
            {
                if (Button("Clear"))
                    GameProgressionDatabase.Clear();
            }
        }
    }
}