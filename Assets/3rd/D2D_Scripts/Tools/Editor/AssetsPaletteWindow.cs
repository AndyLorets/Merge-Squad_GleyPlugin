using D2D.Tools;
using D2D.Utilities;
using UnityEditor;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Tools
{
    public class AssetsPaletteWindow : EditorWindow
    {
        private Editor _editor;
        private AssetsPalette _attachedPalette;

        private static AssetsPaletteWindow _currentWindow;

        private GameObject[] _lastGameObjects;

        [MenuItem("D2D/Open Palette &e")]
        public static void Init()
        {
            if (_currentWindow != null)
            {
                _currentWindow?.Close();
                _currentWindow = null;
            }
            else
            {
                _currentWindow = ScriptableObject.CreateInstance<AssetsPaletteWindow>();
                _currentWindow.position = new Rect(_coreData.tools.palettePosition.x, _coreData.tools.palettePosition.y, _coreData.tools.paletteSize.x, _coreData.tools.paletteSize.y);
                _currentWindow.ShowPopup();
            }

            OnWindowToolsOpen();
        }

        [MenuItem("D2D/Close Palette &d")]
        private static void CloseAll() 
        {
            GetWindow(typeof(AssetsPaletteWindow))?.Close();
        }

        private void OnEnable()
        {
            _attachedPalette = AssetsPalette.Instance;
            _editor = Editor.CreateEditor(_attachedPalette);
        }

        private static void OnWindowToolsOpen()
        {
            // AssetsPalette.Instance.materialTool.OnWindowOpen();
            // AssetsPalette.Instance.meshTool.OnWindowOpen();
        }
        
        private static void OnWindowToolsUpdate()
        {
            // AssetsPalette.Instance.materialTool.ApplyChanges();
            // AssetsPalette.Instance.meshTool.ApplyChanges();
        }

        private void OnGUI()
        {
            // GUILayout.Space(100);

            _editor.OnInspectorGUI();
            
            // GUILayout.Space(20);

            var palette = AssetsPalette.Instance;
            
            if (!Selection.gameObjects.IsNullOrEmpty() && !_lastGameObjects.IsNullOrEmpty() && Selection.gameObjects[0].GetInstanceID() != _lastGameObjects[0].GetInstanceID())
                OnWindowToolsOpen();

            _lastGameObjects = Selection.gameObjects;

            OnWindowToolsUpdate();

            if (GUILayout.Button("Replace"))
            {
                palette.replaceTool.Replace();
            }
            
            if (GUILayout.Button("Export"))
            {
                Selection.objects = palette.toExport;
                EditorApplication.ExecuteMenuItem("Assets/Export Package...");
            }

            GUILayout.Space(20);

            /*GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Player"))
            {
                Shortcuts.SelectPlayer();
            }
            
            if (GUILayout.Button("Level"))
            {
                Shortcuts.SelectLevel();
            }
            
            if (GUILayout.Button("Canvas"))
            {
                Shortcuts.SelectCanvas();
            }

            HierarchyKeeper.Keep();
            
            GUILayout.EndHorizontal();*/
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Close"))
            {
                CloseAll();
            }
            GUI.backgroundColor = Color.white;
            
            // EditorGUILayout.LabelField("");
        }
    }
}