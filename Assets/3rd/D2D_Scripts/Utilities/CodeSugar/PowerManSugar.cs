using System;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public static class PowerManSugar
    {
        public static float DistanceTo(this Vector3 from, Vector3 to)
        {
            return (from - to).magnitude;
        }

        public static float DistanceTo(this Transform from, Transform to)
        {
            return (from.position - to.position).magnitude;
        }
        
        public static float Abs(this float f)
        {
            return Mathf.Abs(f);
        }
        
        public static int Abs(this int f)
        {
            return Mathf.Abs(f);
        }
        
        
        public static bool FlipCoin(this int f)
        {
            return DMath.RandomSign() > 0;
        }
        
        public static int RandomSign(this int f)
        {
            return DMath.RandomSign();
        }
        
        public static bool FlipCoin()
        {
            return DMath.RandomSign() > 0;
        }
        
        public static bool Yes(this float chance)
        {
            return DMath.Random(0f, 1f) <= chance;
        }
        
        public static bool No(this float chance)
        {
            return !chance.Yes();
        }
        
        public static Vector3 RandomPointInsideBoxY0(this float a, bool includeNegative = true)
        {
            return DMath.RandomPointInsideBox(a, 0, a, includeNegative);
        }
        
        public static Vector3 RandomPointInsideBox(this Vector3 v, bool includeNegative = true)
        {
            int k = includeNegative ? 1 : 0;
            float randomX = DMath.Random(-v.x * k, v.x);
            float randomY = DMath.Random(-v.y * k, v.y);
            float randomZ = DMath.Random(-v.z * k, v.z);
            return new Vector3(randomX, randomY, randomZ);
        }
        
        public static void AddForceAndTorque(this Rigidbody rb, Transform target, Vector2 force, Vector2 torque)
        {
            var d = target.position - rb.transform.position;

            rb.AddForce(d.normalized * force.RandomFloat(), ForceMode.Impulse);
            rb.AddTorque(DMath.RandomPointInsideBox(torque.RandomFloat()), ForceMode.Impulse);
        }
        
        public static void AddExplosiveForceAndTorque(this Rigidbody rb, Transform target, Vector2 force, Vector2 r, Vector2 u, Vector2 torque)
        {
            rb.AddExplosionForce(force.RandomFloat(), target.position, r.RandomFloat(), u.RandomFloat());
            rb.AddTorque(DMath.RandomPointInsideBox(torque.RandomFloat()), ForceMode.Impulse);
        }
    }
}