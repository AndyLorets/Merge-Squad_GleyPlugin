using UnityEngine;
using D2D;
using D2D.Utilities;
using DG.Tweening;
using UnityEditor;

[CustomEditor(typeof(PrefabPlacer))]
public class PrefabPlacerEditor : SuperEditor
{
    private static bool _isEditMode;

    void OnSceneGUI()
    {
        Event e = Event.current;
        if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Q)
            _isEditMode = !_isEditMode;
        
        if (!_isEditMode) 
            return;
        
        EditorApplication.update += () =>
        {
            var placer = target as PrefabPlacer;

            if (placer == null || !_isEditMode)
                return;
            
            Selection.activeGameObject = placer.gameObject;
        };

        if (Event.current.type == EventType.MouseDown)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                var placer = target as PrefabPlacer;

                if (placer == null || placer.Prefabs.IsNullOrEmpty())
                    return;

                var prefab = placer.Prefabs.GetRandomElement();
                var instance = Instantiate(prefab);
                instance.transform.position = hitInfo.point + placer.Offset;

                EditorUtility.SetDirty(instance);

                Selection.activeGameObject = placer.gameObject;
            }

            Event.current.Use();
        }

    }

    public override void OnInspectorGUI()
    {
        Event e = Event.current;
        if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Q)
            _isEditMode = !_isEditMode;
        
        if (_isEditMode)
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Disable Editing"))
            {
                _isEditMode = false;
            }
            GUI.backgroundColor = Color.white;
        }
        else
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Enable Editing"))
            {
                _isEditMode = true;

            }
            GUI.backgroundColor = Color.white;
        }
        
        ShowProperty("_prefabs");
        ShowProperty("_offset");
        
        serializedObject.ApplyModifiedProperties();
    }
}