using System.Collections.Generic;
using UnityEngine;

namespace D2D.Utilities
{
    public static class TransformSugar
    {
        public static List<Transform> GetChildTransforms(this Transform target)
        {
            var childList = new List<Transform>();
            foreach (Transform child in target)
            {
                childList.Add(child);
            }

            return childList;
        }
        
        public static Transform GetLastChild(this Transform target)
        {
            return target.childCount == 0 ? null : target.GetChild(target.childCount - 1);
        }
        
        public static void ClearChildren(this Transform target)
        {
            var all = target.GetChildTransforms();
            for (var i = 0; i < all.Count; i++)
                GameObject.DestroyImmediate(all[i].gameObject);
        }
    }
}