using System.Linq;
using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace D2D.Tools
{
    public class Shortcuts : EditorWindow
    {
        public static bool IsShift => CoreSettings.Instance.tools.focusAlsoOnGameObjects;

        [InitializeOnLoadMethod]
        static void EditorInit()
        {
            System.Reflection.FieldInfo info = typeof (EditorApplication).GetField ("globalEventHandler", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
 
            EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue (null);
 
            value += EditorGlobalKeyPress;
 
            info.SetValue (null, value);

            var level = FindObjectOfType<Level>();
            if (level != null)
                level.AutoDetect();
        }
        
        static void EditorGlobalKeyPress()
        {
           //  IsShift = (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.RightShift);
        }
        
        [MenuItem("D2D/Reset transform %/")]
        private static void ResetTransform()
        {
            GameObject obj = Selection.activeGameObject;
            
            if (obj == null)
                return;

            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            // Debug.Log("Transform reseted.");
        }

        [MenuItem("D2D/Select Game Settings &o")]
        public static void SelectGameSettings()
        {
            Selection.activeObject = GameplaySettings.Instance;
        }
        
        [MenuItem("D2D/Select Core Settings &i")]
        public static void SelectCoreSettings()
        {
            Selection.activeObject = CoreSettings.Instance;
        }

        [MenuItem("D2D/SelectParent %'")]
        public static void SelectParent()
        {
            if (Selection.activeGameObject == null || Selection.activeGameObject.transform.parent == null)
                return;

            var parent = Selection.activeTransform.parent;
            Selection.activeTransform = parent;
            // EditorGUIUtility.PingObject(parent.gameObject);
        }
        
        [MenuItem("D2D/Make parent in 0 %0")]
        public static void Group0()
        {
            if (Selection.gameObjects.IsNullOrEmpty())
                return;

            Transform parent = Selection.activeGameObject.transform.parent;
            var group = new GameObject("Group");
            group.transform.position = Vector3.zero;
            group.transform.parent = parent;

            Selection.gameObjects.ForEach(g => g.transform.parent = group.transform);
        }
        
        [MenuItem("D2D/Make parent in middle %m")]
        public static void GroundMiddle()
        {
            if (Selection.gameObjects.IsNullOrEmpty())
                return;

            Vector3 middle = Vector3.zero;
            foreach (GameObject o in Selection.gameObjects)
            {
                middle += o.transform.position;
            }
            middle /= Selection.gameObjects.Length;

            Transform parent = Selection.activeGameObject.transform.parent;
            var group = new GameObject("Group");
            group.transform.position = middle;
            group.transform.parent = parent;
            
            Selection.gameObjects.ForEach(g => g.transform.parent = group.transform);
        }
        
        /*[MenuItem("D2D/FoldParent %]")]
        public static void FoldParent()
        {
            if (Selection.activeGameObject == null || Selection.activeGameObject.transform.parent == null)
            {
                EditorApplication.ExecuteMenuItem("GameObject/Collapse All");
                return;
            }

            var parent = Selection.activeTransform.parent;
            Selection.activeGameObject = parent.gameObject;
            EditorApplication.ExecuteMenuItem("GameObject/Collapse All");
        }*/
        
        [MenuItem("D2D/ToggleCanvases")]
        public static void ClearDB()
        {
            GameProgressionDatabase.Clear();
        }
        
        [MenuItem("D2D/Select Child Material &m")]
        public static void SelectChildrenMaterial()
        {
            if (Selection.activeGameObject == null)
                return;

            var renderer = Selection.activeGameObject.ChildrenGet<Renderer>();

            if (renderer == null)
                return;
            
            Selection.activeObject = renderer.sharedMaterial;

            if (IsShift)
            {
                // Debug.Log("HI");
                Selection.activeGameObject = renderer.gameObject;
            }
                
            FocusProjectWindow();
        }

        [MenuItem("D2D/Select Child Mesh &l")]
        public static void SelectChildrenMesh()
        {
            if (Selection.activeGameObject == null)
                return;

            var filter = Selection.activeGameObject.ChildrenGet<MeshFilter>();
            var skin = Selection.activeGameObject.ChildrenGet<SkinnedMeshRenderer>();

            if (filter != null)
            {
                Selection.activeObject = filter.sharedMesh;
                
                if (IsShift)
                    Selection.activeGameObject = filter.gameObject;
            }
            else if (skin != null)
            {
                Selection.activeObject = skin.sharedMesh;
                
                if (IsShift)
                    Selection.activeGameObject = skin.gameObject;
            }
                
            FocusProjectWindow();
        }
        
        [MenuItem("D2D/Select Scene &j")]
        public static void SelectScene()
        {
            var path = SceneManager.GetActiveScene().path;
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            FocusProjectWindow();
        }

        private static void FocusProjectWindow()
        {
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        [MenuItem("D2D/Select groun &g")]
        public static void SelectGround()
        {
            SelectByTag("Ground");
        }
        
        [MenuItem("D2D/Select player &p")]
        public static void SelectPlayer()
        {
            SelectByType<Player>();
        }

        // [MenuItem("D2D/Select player %&;")]
        public static void SelectCanvas()
        {
            SelectByType<MainCanvasMark>();
        }
        
        // [MenuItem("D2D/Select player %&l")]
        public static void SelectLevel()
        {
            SelectByType<Level>();
        }

        [MenuItem("D2D/Select Tag1 %1")]
        public static void SelectTag1() => SelectByType<Tag1>();
        
        [MenuItem("D2D/Select Tag2 %2")]
        public static void SelectTag2() => SelectByType<Tag2>();
        
        [MenuItem("D2D/Select Tag3 %3")]
        public static void SelectTag3() => SelectByType<Tag3>();

        [MenuItem("D2D/Select Tag4 %4")]
        public static void SelectTag4() => SelectByType<Tag4>();


        // keep
        
        private static void SelectByType<T>() where T : Component
        {
            var objectOfDesiredType = Resources.FindObjectsOfTypeAll<T>().Select(o => o.gameObject).ToArray();

            if (!objectOfDesiredType.IsNullOrEmpty())
                Selection.objects = objectOfDesiredType;
        }

        private static void SelectByTag(string tag)
        {
            var objectsWithDesiredTag = GameObject.FindGameObjectsWithTag(tag);

            if (!objectsWithDesiredTag.IsNullOrEmpty())
                Selection.objects = objectsWithDesiredTag;
        }
    }
}