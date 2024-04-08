using System.Collections.Generic;
using System.Linq;
using D2D.Core;
using D2D.Utils;
using UnityEngine;

namespace D2D.Utilities
{
    public static class DRay
    {
        /// <summary>
        /// Just raycast simplifier.
        /// </summary>
        public static RaycastHit? GetHit(this Transform transform, Vector3 direction, float length, LayerMask layerMask)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, length, layerMask))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
                if (CoreSettings.Instance.raycastDebugPauseEnabled)
                    Debug.Break();
                return hit;
            }
            
            Debug.DrawRay(transform.position, direction * length, Color.red);
            if (CoreSettings.Instance.raycastDebugPauseEnabled)
                Debug.Break();
            return null;
        }
        
        public static RaycastHit? GetForwardHit(this Transform transform, float length, LayerMask layerMask)
        {
            return GetHit(transform, transform.forward, length, layerMask);
        }
        
        public static bool IsForwardHit(this Transform transform, float length, LayerMask layerMask)
        {
            return GetHit(transform, transform.forward, length, layerMask) != null;
        }
        
        public static bool IsHit(this Transform transform, Vector3 direction, float length, LayerMask layerMask)
        {
            return GetHit(transform, direction, length, layerMask) != null;
        }
        
        /// <summary>
        /// For instance useful if we check everything around player (like a flower ray around him).
        /// normal = new Vector(1, 0, 0) in original
        /// </summary>
        public static RaycastHit?[] GetHitAngular(this Transform transform, Vector3 normal, float length, LayerMask layerMask, int raysCount = 10)
        {
            float anglePerRay = 360f / raysCount;
            var hitAngles = new List<RaycastHit?>();
            
            for (int i = 0; i < raysCount; i++)
            {
                float angle = anglePerRay * i;
                var direction = Quaternion.Euler(0, angle, 0) * normal;

                var hit = GetHit(transform, direction, length, layerMask);
                if (hit != null)
                    hitAngles.Add(hit);
            }

            return hitAngles.ToArray();
        }
        
        public static float? GetAverageHitAngle(this Transform transform, Vector3 normal, float length, LayerMask layerMask, int raysCount = 10)
        {
            float anglePerRay = 360f / raysCount;
            var hitAngles = new List<float>();
            
            for (int i = 0; i < raysCount; i++)
            {
                float angle = anglePerRay * i;
                var direction = Quaternion.Euler(0, angle, 0) * normal;
                
                if (IsHit(transform, direction, length, layerMask))
                    hitAngles.Add(angle);
            }

            if (hitAngles.IsNullOrEmpty())
                return null;
            
            return hitAngles.Average();
        }

        public static bool IsHitAngular(this Transform transform, Vector3 normal, float length, LayerMask layerMask, int raysCount = 10)
        {
            return !GetHitAngular(transform, normal, length, layerMask, raysCount).IsNullOrEmpty();
        }
    }
}