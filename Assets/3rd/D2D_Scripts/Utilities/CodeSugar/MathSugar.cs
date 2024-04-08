using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Utilities
{
    public static class MathSugar
    {
        
        
        public static Vector3 AngleToVector(this Vector3 angle, Transform t)
        {
            return Quaternion.Euler(angle) * t.forward;
        }
        
        public static float Max(this Vector2 v)
        {
            return Mathf.Max(v.x, v.y);
        }
        
        public static bool Between(this float i, float a, float b) => i >= a && i <= b;
        
        public static float Angle360(Vector2 p1, Vector2 p2, Vector2 o = default)
        {
            Vector2 v1, v2;
            if (o == default)
            {
                v1 = p1.normalized;
                v2 = p2.normalized;
            }
            else
            {
                v1 = (p1 - o).normalized;
                v2 = (p2 - o).normalized;
            }
            float angle = Vector2.Angle(v1, v2);
            return Mathf.Sign(Vector3.Cross(v1, v2).z) < 0 ? (360 - angle) % 360 : angle;
        }
        
        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }
        
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static float Clamp(this float value, Vector2 v)
        {
            return Mathf.Clamp(value, v.x, v.y);
        }
        
        public static Vector3 SignedClamp(this Vector3 v, Vector3 limits)
        {
            return new Vector3(v.x.SignedClamp(limits.x), v.y.SignedClamp(limits.y), v.z.SignedClamp(limits.z));
        }
        
        public static float SignedClamp(this float value, float range)
        {
            return Mathf.Clamp(value, -range, range);
        }
        
        public static float LimitMax(this float value, float max)
        {
            return Mathf.Min(value, max);
        }
        
        public static int LimitMax(this int value, int max)
        {
            return Mathf.Min(value, max);
        }
        
        public static float LimitMin(this float value, float min)
        {
            return Mathf.Max(value, min);
        }
        
        public static int LimitMin(this int value, int min)
        {
            return Mathf.Max(value, min);
        }
        
        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static float FactorRange(this float factor, float min, float max)
        {
            var a = Mathf.Max(min, max);
            var b = Mathf.Min(min, max);
            
            return a + (a - b) * factor;
        }

        #region Transform

        public static void LookAtOnlyY(this Transform origin, Transform lookAtTarget, float offsetY = 0)
        {
            var p = lookAtTarget.position;
            p.y = origin.position.y;
            origin.LookAt(p);
            origin.eulerAngles += new Vector3(0, offsetY);
        }

        public static Vector3 SetY(this Vector3 target, float value)
        {
            target.y = value;
            return target;
        }

        public static float DistanceTo(this Vector3 from, Vector3 to)
        {
            return (from - to).magnitude;
        }

        public static float DistanceTo(this Transform from, Transform to)
        {
            return (from.position - to.position).magnitude;
        }

        public static bool InRange(this Transform from, Transform to, float d)
        {
            return (from.position - to.position).sqrMagnitude < d * d;
        }
        
        public static bool InRange(this Transform from, Vector3 to, float d)
        {
            return (from.position - to).sqrMagnitude < d * d;
        }
        
        public static bool InRangeIgnoreY(this Transform from, Vector3 to, float d)
        {
            var fromNoY = from.position;
            fromNoY.y = to.y;
            return (fromNoY - to).sqrMagnitude < d * d;
        }
        
        public static Transform Closest(this Transform from, Transform[] to)
        {
            float minDistance = float.MaxValue;
            var closest = to[0];
            
            for (var i = 0; i < to.Length; i++)
            {
                var d = (from.position - to[i].position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = to[i];
                }
            }

            return closest;
        }

        public static Transform Closest(this Transform from, List<Transform> to)
        {
            float minDistance = float.MaxValue;
            var closest = to[0];

            for (var i = 0; i < to.Count; i++)
            {
                var d = (from.position - to[i].position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = to[i];
                }
            }
            
            return closest;
        }
        
        public static Transform Closest(this Transform from, Component[] to)
        {
            float minDistance = float.MaxValue;
            var closest = to[0];
            
            for (var i = 0; i < to.Length; i++)
            {
                var d = (from.position - to[i].transform.position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = to[i];
                }
            }

            return closest.transform;
        }
        
        public static Transform Closest(this Transform from, List<Component> to)
        {
            float minDistance = float.MaxValue;
            var closest = to[0];
            
            for (var i = 0; i < to.Count; i++)
            {
                var d = (from.position - to[i].transform.position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = to[i];
                }
            }

            return closest.transform;
        }
        
        public static T Closest<T>(this Transform from, IEnumerable<T> targets)where T: Component
        {
            float minDistance = float.MaxValue;
            var closest = targets.First();
            
            foreach (T t in targets)
            {
                var d = (from.position - t.transform.position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = t;
                }
            }
            return closest;
        }
        
        public static T Closest<T>(this Transform from, List<T> to) where T: Component
        {
            float minDistance = float.MaxValue;
            var closest = to[0];
            
            for (var i = 0; i < to.Count; i++)
            {
                var d = (from.position - to[i].transform.position).sqrMagnitude;
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = to[i];
                }
            }

            return closest;
        }
        
        public static T ClosestOfType<T>(this Transform from, List<Collider> to) where T: Component
        {
            float minDistance = float.MaxValue;
            var closest = to[0];
            T component = null;
            T closestComponent = null;
            
            for (var i = 0; i < to.Count; i++)
            {
                if (to[i] == null)
                {
                    continue;
                }

                var d = (from.position - to[i].transform.position).sqrMagnitude;
                component = to[i].GetComponent<T>();
                if (d < minDistance && component != null)
                {
                    closestComponent = component;
                    minDistance = d;
                    closest = to[i];
                }
            }

            return closestComponent;
        }
        
        public static Vector3 AveragePosition(this IEnumerable<Transform> transforms)
        {
            var array = transforms.ToArray();
            var x = array.Average(t => t.position.x);
            var y = array.Average(t => t.position.y);
            var z = array.Average(t => t.position.z);
            return new Vector3(x, y, z);
        }
        
        public static Vector3 Average(this Vector3[] vectors)
        {
            var x = vectors.Average(v => v.x);
            var y = vectors.Average(v => v.y);
            var z = vectors.Average(v => v.z);

            return new Vector3(x, y, z);
        }
        
        public static Vector3 Average(this IEnumerable<Vector3> vectors)
        {
            var enumerable = vectors as Vector3[] ?? vectors.ToArray();
            var x = enumerable.Average(v => v.x);
            var y = enumerable.Average(v => v.y);
            var z = enumerable.Average(v => v.z);

            return new Vector3(x, y, z);
        }
        
        public static float DistanceToPlayer(this Transform t)
        {
            return (t.position - _player.transform.position).magnitude;
        }
        
        public static void LookAtOnlyY(this Transform origin, Vector3 lookAtTarget, Vector3? axis = null)
        {
            var p = lookAtTarget;
            p.y = origin.position.y;
            if (axis == null)
                origin.LookAt(p);
            else
            {
                origin.LookAt(p, axis.Value);
            }
        }

        #endregion
    }
}