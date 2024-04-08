using UnityEngine;
using D2D.Utilities;
using UnityEditor;

namespace D2D
{
    public class HierarchyFolder : SmartScript
    {
        [SerializeField] private bool _forceToClose = true;

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            return;
            
            if (Selection.activeGameObject == null || Selection.activeGameObject.transform.parent == null || !_forceToClose)
                return;
            
            var folder = Selection.activeGameObject.GetComponentInParent<HierarchyFolder>();
            if (folder != null)
            {
                Selection.activeGameObject = folder.gameObject;
                EditorApplication.ExecuteMenuItem("GameObject/Collapse All");
                // EditorGUIUtility.PingObject(folder.gameObject);
            }

            /*var selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
            foreach(var go in selection)
                Collapse(go, true);*/
        }
        
        #endif
    }
}