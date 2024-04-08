using System;
using D2D.Databases;
using UnityEngine;

namespace D2D.Utilities
{
    /// <summary>
    /// Contains common math operations and better random.
    /// </summary>
    public static class DMath
    {
        private static DataContainer<int> _seed = new DataContainer<int>(nameof(_seed), 0);
        
        /// <summary>
        /// Sets the random seed.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            var seed = (int) (Time.deltaTime * 1000 * _seed.Value);
            
            _seed.Value++;

            UnityEngine.Random.InitState(seed);
        }

        /// <summary>
        /// Returns the random float in [min, max].
        /// </summary>
        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        
        public static float RandomByAmplitude(float a)
        {
            return Random(-a, a);
        }
        
        /// <summary>
        /// Returns the random integer in [min, max].
        /// </summary>
        public static int Random(int min, int max)
        {
            float randomValue = Random((float) min, (float) max);
            return (int) Math.Round(randomValue, 0);
        }
        
        /// <summary>
        /// Returns 1 or -1. 
        /// </summary>
        public static int RandomSign()
        {
            return Random(0, 1f) > .5f ? 1 : -1;
        }
        
        /// <summary>
        /// Returns the random point in box [vector a: x/y/z, vector b: x/y/z].
        /// </summary>
        public static Vector3 RandomPointInsideBox(Vector3 a, Vector3 b)
        {
            return new Vector3(Random(a.x, b.x), Random(a.y, b.y), Random(a.z, b.z));
        }

        public static Vector3 RandomPointInsideBoxCollider(this Transform transform, BoxCollider box)
        {
            return transform.position + -box.center + RandomPointInsideBox(-box.size / 2f, box.size / 2f);
        }
        
        public static Quaternion RandomRotation()
        {
            return Quaternion.Euler(RandomPointInsideBox(360));
        }

        /// <summary>
        /// Return the random point in box with width (x), height (y) and length (z). [0, x/y/z].
        /// </summary>
        public static Vector3 RandomPointInsideBox(float x, float y, float z, bool includeNegative = true)
        {
            int k = includeNegative ? 1 : 0;
            float randomX = Random(-x * k, x);
            float randomY = Random(-y * k, y);
            float randomZ = Random(-z * k, z);
            return new Vector3(randomX, randomY, randomZ);
        }
        
        /// <summary>
        /// Returns the random point in box with width (a), height (a) and length (a).
        /// </summary>
        public static Vector3 RandomPointInsideBox(float a, bool includeNegative = true)
        {
            return RandomPointInsideBox(a, a, a, includeNegative);
        }
        
        public static Vector3 RandomPointInsideBox2D(float a, bool includeNegative = true)
        {
            return RandomPointInsideBox(a, a, 0, includeNegative);
        }
        
        public static float RandomFloat(this Vector2 range)
        {
            return Random(range.x, range.y);
        }
        
        public static int RandomInt(this Vector2 range)
        {
            return Random((int) range.x, (int)range.y);
        }

        /// <summary>
        /// Round float to int
        /// </summary>
        public static int Round(this float val)
        {
            return (int) Math.Round(val, 0);
        }
        
        /// <summary>
        /// Are numbers differ less by 0.01?
        /// </summary>
        public static bool Almost(this float a, float b, float tolerance = .01f)
        {
            return Mathf.Abs(a - b) < tolerance;
        }
        
        /// <summary>
        /// Is number differ from zero less by 0.01?
        /// </summary>
        public static bool AlmostZero(this float a, float tolerance = .01f)
        {
            return Almost(a, 0, tolerance);
        }

        /// <summary>
        /// Is distance between tow points (a and b) less than threshold.
        /// </summary>
        public static bool Almost(this Vector3 a, Vector3 b, float threshold = .01f)
        {
            return (a - b).magnitude.AlmostZero(threshold);
        }
        
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }
    }
}

