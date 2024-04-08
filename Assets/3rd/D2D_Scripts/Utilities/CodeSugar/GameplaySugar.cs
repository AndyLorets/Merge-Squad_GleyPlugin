using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using D2D.Animations;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace D2D.Utilities
{
    public static class GameplaySugar
    {
        public static bool IsTimeCountAppropriate(this int frames)
        {
            return Time.frameCount % frames == 0;
        }
        
        public static Tween SineEase(this Tween t)
        {
            return t.SetEase(Ease.InOutSine);
        }
        
        public static T ParentGetRecursive<T>(this Transform g) where T : Component
        {
            Transform root = g;
            T component = null;
            do
            {
                component = root.Get<T>();
                root = root.transform.parent;

                if (component != null || root == null)
                    break;

            } while (true);

            return component;
        }
        
        public static Collider[] GetOverlaps(this BoxCollider b)
        {
            return Physics.OverlapBox(b.transform.position + b.center, b.size * b.transform.localScale.x,
                b.transform.rotation);
        }

        public static UniTask Seconds(this float f)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(f));
        }
        
        public static void PlayAll(this DAnimation[] animations)
        {
            animations.ForEach(a => a.Play());
        }

        public static void OnAll(this Transform[] animations) => 
            animations.ForEach(a => a.On());
        
        public static void OnAll(this List<Transform> animations) =>
            animations.ForEach(a => a.On());
        
        public static void OnAll(this IEnumerable<Transform> animations) =>
            animations.ForEach(a => a.On());
        
        public static void OffAll(this Transform[] animations) => 
            animations.ForEach(a => a.Off());
        
        public static void OffAll(this List<Transform> animations) =>
            animations.ForEach(a => a.Off());
        
        public static void OffAll(this IEnumerable<Transform> animations) =>
            animations.ForEach(a => a.Off());
        
        public static void OnAll(this GameObject[] animations) => 
            animations.ForEach(a => a.On());
        
        public static void OnAll(this List<GameObject> animations) =>
            animations.ForEach(a => a.On());
        
        public static void OnAll(this IEnumerable<GameObject> animations) =>
            animations.ForEach(a => a.On());
        
        public static void OffAll(this GameObject[] animations) => 
            animations.ForEach(a => a.Off());
        
        public static void OffAll(this List<GameObject> animations) =>
            animations.ForEach(a => a.Off());
        
        public static void OffAll(this IEnumerable<GameObject> animations) =>
            animations.ForEach(a => a.Off());

        #region All

        public static bool Is<T>(this Collision other) where T:Component =>
            Is<T>(other.gameObject);
        
        public static bool Is<T>(this Component other) where T : Component =>
            Is<T>(other.gameObject);

        public static bool Is<T>(this GameObject other) where T:Component => 
            other != null && other.TryGetComponent(out T t);
        

        public static T Get<T>(this Collision other) where T:Component
        {
            other.gameObject.TryGetComponent(out T t);
            return t;
        }

        public static void IfCanGet<T>(this Component other, Action<T> func) where T:Component
        {
            var t = other.GetComponent<T>();
            if (t != null)
            {
                func?.Invoke(t);
            }
        }
        
        public static void IfCanChildrenGet<T>(this Component other, Action<T> func) where T:Component
        {
            var t = other.gameObject.ChildrenGet<T>();
            if (t != null)
                func?.Invoke(t);
        }
        
        public static void IfCanGet<T>(this Collision other, Action<T> func) where T:Component
        {
            var t = other.gameObject.GetComponent<T>();
            if (t != null)
            {
                func?.Invoke(t);
            }
        }
        
        public static void IfNull<T>(this T other, Action<T> func) where T:Component
        {
            if (other == null)
                func?.Invoke(other);
        }
        
        public static void IfNotNull<T>(this T other, Action<T> func) where T:Component
        {
            if (other != null)
                func?.Invoke(other);
        }
        
        public static T Important<T>(this T other, GameObject g = null) where T:Component
        {
            return NotNull(other, g);
        }

        public static T NotNull<T>(this T other, GameObject g = null) where T:Component
        {
            if (other != null)
                return other;

            Debug.LogError(g != null
                ? $"{typeof(T).FullName} not found for {g.name}!"
                : $"{typeof(T).FullName} not found!");

            return null;
        }

        public static Tween AfterCall(this float delay, Action a)
        {
            return DOVirtual.DelayedCall(delay, () => a?.Invoke());
        }

        #endregion

        #region GO
        
        public static void IfCanFind<T>(this Component other, Action<T> func) where T:Component
        {
            var t = GameObject.FindObjectOfType<T>();
            if (t != null)
            {
                func?.Invoke(t);
            }
        }


        public static T Find<T>(this Component other, bool canBeNull = true) where T:Component =>
            Single(other.gameObject, GameObject.FindObjectOfType<T>, canBeNull);

        public static T Find<T>(this GameObject other, bool canBeNull = true) where T : Component =>
            Single(other, GameObject.FindObjectOfType<T>, canBeNull);
        
        public static T[] Finds<T>(this Component other, bool canBeNull = true) where T:Component =>
            Many(other.gameObject, GameObject.FindObjectsOfType<T>, canBeNull);

        public static T[] Finds<T>(this GameObject other, bool canBeNull = true) where T : Component =>
            Many(other, GameObject.FindObjectsOfType<T>, canBeNull);

        public static T[] Finds<T>(this Component other, Action<T> a, bool canBeNull = true) where T : Component
        {
            var r = Many(other.gameObject, GameObject.FindObjectsOfType<T>, canBeNull);
            r.ForEach(a);
            return r;
        }

        public static T Get<T>(this Component other, bool canBeNull = true) where T:Component => 
            Single(other.gameObject, other.GetComponent<T>, canBeNull);

        public static T Get<T>(this GameObject other, bool canBeNull = true) where T:Component =>
            Single(other, other.GetComponent<T>, canBeNull);


        public static T[] Gets<T>(this Component other, bool canBeNull = true) where T:Component => 
            Many(other.gameObject, other.GetComponents<T>, canBeNull);

        public static T[] Gets<T>(this GameObject other, bool canBeNull = true) where T:Component =>
            Many(other, other.GetComponents<T>, canBeNull);
        
        public static T[] Gets<T>(this Component other, Action<T> a, bool canBeNull = true) where T:Component
        {
            var r = Many(other.gameObject, other.GetComponents<T>, canBeNull);
            r.ForEach(a);
            return r;
        }


        public static T ChildrenGet<T>(this Component other, bool canBeNull = true) where T:Component =>
            Single(other.gameObject, other.GetComponentInChildren<T>, canBeNull);

        public static T ChildrenGet<T>(this GameObject other, bool canBeNull = true) where T:Component => 
            Single(other, other.GetComponentInChildren<T>, canBeNull);


        public static T[] ChildrenGets<T>(this Component other, bool canBeNull = true) where T:Component => 
            Many(other.gameObject, other.GetComponentsInChildren<T>, canBeNull);

        public static T[] ChildrenGets<T>(this GameObject other, bool canBeNull = true) where T:Component =>
            Many(other, other.GetComponentsInChildren<T>, canBeNull);
        
        public static T[] ChildrenGets<T>(this Component other, Action<T> a, bool canBeNull = true) where T:Component
        {
            var r = Many(other.gameObject, other.GetComponentsInChildren<T>, canBeNull);
            r.ForEach(a);
            return r;
        }


        public static T ParentGet<T>(this Component other, bool canBeNull = true) where T:Component => 
            Single(other.gameObject, other.GetComponentInParent<T>, canBeNull);

        public static T ParentGet<T>(this GameObject other, bool canBeNull = true) where T:Component =>
            Single(other, other.GetComponentInParent<T>, canBeNull);


        public static T[] ParentGets<T>(this Component other, bool canBeNull = true) where T:Component =>
            Many(other.gameObject, other.GetComponentsInParent<T>, canBeNull);

        public static T[] ParentGets<T>(this GameObject other, bool canBeNull = true) where T : Component =>
            Many(other, other.GetComponentsInParent<T>, canBeNull);
        
        public static T[] ParentGets<T>(this Component other, Action<T> a, bool canBeNull = true) where T : Component
        {
            var r = Many(other.gameObject, other.GetComponentsInParent<T>, canBeNull);
            r.ForEach(a);
            return r;
        }

        private static T Single<T>(GameObject owner, Func<T> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception($"Single NOT found {typeof(T).FullName} for {owner.name}!");
            
            return result;
        }

        private static T[] Many<T>(GameObject owner, Func<T[]> getComponentMethod, bool canBeNull) 
            where T : Component
        {
            var result = getComponentMethod?.Invoke();
            if (!canBeNull && result == null)
                throw new Exception($"Many NOT found {typeof(T).FullName} for {owner.name}!");
            return result;
        }

        #endregion
        
        #region Layers
        
        public static int ToLayer(this LayerMask layerMask) 
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            while(layer > 0) {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }
        
        public static void SetLayerRecursively(this GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }
       
            obj.layer = newLayer;
       
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        
        #endregion

        #region GameObject SetActive, Tranform, Parenting

        public static void DeattachOn(this GameObject target) =>
            DeattachOn(target.transform);

        public static void DeattachOn(this Transform target)
        {
            target.transform.SetParent(null);
            target.gameObject.On();
        }

        /*public static void On(this Component target, bool canBeNull = true) => 
            target.gameObject.On();

        public static void On(this Component target, float after, bool canBeNull = true) => 
            target.gameObject.On();*/

        public static void On(this Transform target, bool canBeNull = true) => 
            target.gameObject.On();

        public static void On(this Transform target, float after, bool canBeNull = true) => 
            target.gameObject.On();

        public static void On(this GameObject target, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;

            target.SetActive(true);
        }
        
        public static void On(this GameObject target, float after, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;

            DOVirtual.DelayedCall(after, () => On(target, canBeNull));
        }
        
        /*public static void Off(this Component target, bool canBeNull = true) => 
            target.gameObject.On();

        public static void Off(this Component target, float after, bool canBeNull = true) => 
            target.gameObject.On();*/
        
        public static void Off(this Transform target, bool canBeNull = true) => 
            target.gameObject.Off();

        public static void Off(this Transform target, float after, bool canBeNull = true) => 
            target.gameObject.Off();

        public static void Off(this GameObject target, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;
            
            target.SetActive(false);
        }

        public static void Off(this GameObject target, float after, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;

            DOVirtual.DelayedCall(after, () => Off(target, canBeNull));
        }
        
        public static void Reactivate(this GameObject target, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;
            
            target.SetActive(false);
            target.SetActive(true);
        }
        
        public static void Reactivate(this GameObject target, float delay, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;
            
            target.SetActive(false);
            DOVirtual.DelayedCall(delay, () => target.SetActive(true));
        }

        public static void SwitchActivity(this GameObject target, bool canBeNull = true)
        {
            if (!IsObjectValid(target, canBeNull))
                return;
            
            target.SetActive(!target.activeSelf);
        }

        private static bool IsObjectValid(GameObject target, bool canBeNull)
        {
            if (target == null)
            {
                if (canBeNull)
                    return false;
                
                throw new ArgumentNullException("GameObject you want to change activity");
            }

            return true;
        }
        
        // public static void 

        #endregion

        #if UNITY_EDITOR

        public static GameObject EditorInstantiate(this GameObject prefab, Vector3 position, Transform parent = null)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.transform.position = position;
            instance.transform.parent = parent;
            return instance;
        }
        
        public static GameObject EditorInstantiate(this GameObject prefab)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            return instance;
        }
        
        #endif
    }
}