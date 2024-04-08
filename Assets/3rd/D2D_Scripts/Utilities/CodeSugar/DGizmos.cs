using UnityEngine;

namespace D2D
{
    public static class DGizmos
    {
        public static void Sphere(this Vector3 p, float r, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawSphere(p, r);
        }
        
        public static void SphereWire(this Vector3 p, float r, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawSphere(p, r);
        }
        
        public static void Box(this Vector3 p, Vector3 r, Color c)
        {
            Gizmos.color = c;
            Gizmos.DrawCube(p, r);
        }
    }
}