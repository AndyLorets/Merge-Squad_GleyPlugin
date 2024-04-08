using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Utilities
{
    public class SmartScriptAlloc : MonoBehaviour
    {
        protected bool IsAllocationEnabled => _coreData.allocationEnabled && _isAlloc;
        
        private Dictionary<Type, Component> _getCache;

        private Dictionary<Type, Component[]> _getsCache;

        private Dictionary<Type, Component> _parentGetCache;

        private Dictionary<Type, Component[]> _parentGetsCache;

        private Dictionary<Type, Component> _childrenGetCache;

        private Dictionary<Type, Component[]> _childrenGetsCache;

        private Dictionary<Type, Component> _findCache;

        private Dictionary<Type, Component[]> _findsCache;
        
        private bool _canBeNull = false;
        private bool _isAlloc = true;

        public SmartScriptAlloc NoAlloc()
        {
            _isAlloc = false;
            return this;
        }
        
        public SmartScriptAlloc CanBeNull()
        {
            _canBeNull = true;
            return this;
        }

        private void ResetParameters()
        {
            _canBeNull = false;
            _isAlloc = true;
        }

        public T Get<T>(bool canBeNull = false) where T : Component => 
            Single(_getCache, GetComponent<T>, canBeNull);

        public T[] Gets<T>(bool canBeNull = false) where T : Component => 
            Many(_getsCache, GetComponents<T>, canBeNull);

        public T ChildrenGet<T>(bool canBeNull = false) where T : Component => 
            Single(_childrenGetCache, GetComponentInChildren<T>, canBeNull);

        public T[] ChildrenGets<T>(bool canBeNull = false) where T : Component => 
            Many(_childrenGetsCache, GetComponentsInChildren<T>, canBeNull);

        public T ParentGet<T>(bool canBeNull = false) where T : Component => 
            Single(_parentGetCache, GetComponentInParent<T>, canBeNull);

        public T[] ParentGets<T>(bool canBeNull = false) where T : Component => 
            Many(_parentGetsCache, GetComponentsInParent<T>, canBeNull);

        public T Find<T>(bool canBeNull = false) where T : Component => 
            Single(_findCache, FindObjectOfType<T>, canBeNull);

        public T[] Finds<T>(bool canBeNull = false) where T : Component => 
            Many(_findsCache, FindObjectsOfType<T>, canBeNull);

        private T Single<T>(Dictionary<Type, Component> storage, Func<T> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            if (IsAllocationEnabled)
            {
                storage ??= new Dictionary<Type, Component>();

                if (storage.TryGetValue(typeof(T), out var cached))
                {
                    ResetParameters();
                    return (T) cached;
                }
            }

            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception("Component was not found!");

            if (IsAllocationEnabled)
            {
                storage.Add(typeof(T), result);
            }

            ResetParameters();
            return result;
        }

        private T[] Many<T>(Dictionary<Type, Component[]> storage, Func<T[]> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            if (IsAllocationEnabled)
            {
                storage ??= new Dictionary<Type, Component[]>();

                if (storage.TryGetValue(typeof(T), out var cached))
                {
                    ResetParameters();
                    return (T[]) cached;
                }
            }

            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception("Component was not found!");

            if (IsAllocationEnabled)
            {
                storage.Add(typeof(T), result);
            }

            ResetParameters();
            return result;
        }
    }
}