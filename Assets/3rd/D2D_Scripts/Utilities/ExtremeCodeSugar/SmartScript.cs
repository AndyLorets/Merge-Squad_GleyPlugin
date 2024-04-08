using System;
using System.Collections.Generic;
using UnityEngine;

namespace D2D.Utilities
{
    public class SmartScript : MonoBehaviour
    {
        public T Get<T>(bool canBeNull = true) where T : Component => 
            Single(GetComponent<T>, canBeNull);

        public T[] Gets<T>(bool canBeNull = true) where T : Component => 
            Many(GetComponents<T>, canBeNull);
        

        public T ChildrenGet<T>(bool canBeNull = true) where T : Component => 
            Single(GetComponentInChildren<T>, canBeNull);

        public T[] ChildrenGets<T>(bool canBeNull = true) where T : Component => 
            Many(GetComponentsInChildren<T>, canBeNull);
        

        public T ParentGet<T>(bool canBeNull = true) where T : Component => 
            Single(GetComponentInParent<T>, canBeNull);

        public T[] ParentGets<T>(bool canBeNull = true) where T : Component => 
            Many(GetComponentsInParent<T>, canBeNull);
        

        public T Find<T>(bool canBeNull = true) where T : Component => 
            Single(FindObjectOfType<T>, canBeNull);

        public T[] Finds<T>(bool canBeNull = true) where T : Component => 
            Many(FindObjectsOfType<T>, canBeNull);
        
        
        private T Single<T>(Func<T> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception($"Single NOT found {typeof(T).FullName} for {gameObject.name}!");
            
            return result;
        }

        private T[] Many<T>(Func<T[]> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception($"Many NOT found {typeof(T).FullName} for {gameObject.name}!");
            return result;
        }
    }
}