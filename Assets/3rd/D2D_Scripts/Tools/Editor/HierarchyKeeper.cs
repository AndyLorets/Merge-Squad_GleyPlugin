using System;
using System.Collections.Generic;
using System.Linq;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace D2D.Tools
{
    public class HierarchyKeeper : EditorWindow
    {
        private static readonly string[] ExcludingTags = 
        {
            "Respawn"
            ,"Finish"
            ,"Untagged"
            ,"EditorOnly"
            ,"MainCamera"
            ,"Player"
            ,"GameController"
        };

        // private static readonly Type[] SiblingOrder =
        // {
        //     typeof(Level)
        //     ,typeof(Canvas)
        //     ,typeof(Player)
        // };

        private static int _siblingIndex;
        
        private void OnEnable()
        {
            if (!CoreSettings.Instance.tools.isKeeperOn)
                return;

            Keep();
        }

        [MenuItem("D2D/Keep hierarchy &.")]
        public static void Keep()
        {
            if (!CoreSettings.Instance.tools.isKeeperOn)
                return;
            
            SiblingOrderGameObjects();
            SortGameObjectsToFolders();
        }

        private static void SiblingOrderGameObjects()
        {
            ResetSibling();

            PushSibling<Level>();
            PushSibling<Canvas>();

            /*var palette = FindObjectOfType<HelpMePlaceSystem>();
            if (palette != null)
                PushSibling(palette.transform);*/

            var separatorName = CoreSettings.Instance.tools.separatorName;
            var separator = GameObject.Find(separatorName);
            if (separator == null)
                separator = new GameObject(separatorName);

            PushSibling(separator.transform);

            var hierarchyFolders = FindObjectsOfType<HierarchyFolder>();
            foreach (HierarchyFolder f in hierarchyFolders)
                PushSibling(f.transform);

            PushSibling<Player>();
        }

        private static void SortGameObjectsToFolders()
        {
            foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                if (ExcludingTags.Contains(tag))
                    continue;
                
                var tagged = GameObject.FindGameObjectsWithTag(tag);
                if (tagged.IsNullOrEmpty())
                    continue;

                var folderName = $"[Folder] {tag}";
                var folder = GameObject.Find(folderName);
                
                if (folder == null)
                {
                    folder = new GameObject(folderName);
                    folder.AddComponent<HierarchyFolder>();
                }
                
                if (folder.GetComponents<Component>().Length > 2)
                {
                    Debug.LogError($"You have GO named as folder for tags: {folderName}. Please rename it");
                    continue;
                }

                // Move tagged GOs to the folder safely
                try
                {
                    tagged.ForEach(t => t.transform.parent = folder.transform);
                }
                catch (Exception) { }
            }
        }

        private static void PushSibling(Transform target)
        {
            if (target == null)
                return;
            
            target.SetSiblingIndex(_siblingIndex);
            _siblingIndex++;
        }
        
        private static void PushSibling<T>() where T: Component
        {
            PushSibling(FindObjectOfType<T>()?.transform);
        }

        private static void ResetSibling()
        {
            _siblingIndex = 0;
        }
    }
}