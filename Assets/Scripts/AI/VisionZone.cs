/*using System;
using UnityEngine;

namespace D2D.AI
{
    /// <summary>
    /// We will have ignore zone = _gap.
    /// Where nothing will happen, IsInVision() == false && IsOutOfVision() == false
    /// We need this thing to let AI gain enough distance from target
    /// to avoid "boundary vision super fast state change"
    /// </summary>
    [System.Serializable]
    public class VisionZone
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _gap = 3;

        public float Distance => _distance;

        public bool IsInVision(Transform me, Transform other)
        {
            if (ReferenceEquals(me, other))
                return false;
            
            var d = me.position - other.position;
            return d.sqrMagnitude <= Distance * Distance;
        }
        
        public bool IsOutOfVision(Transform me, Transform other)
        {
            if (me == null || other == null)
                throw new NullReferenceException("other or me");

            if (ReferenceEquals(me, other))
                throw new NullReferenceException("try to check distance from yourself!");

            var d = me.position - other.position;

            var a = Distance + _gap;
            return d.sqrMagnitude >= a * a;
        }
    }
}*/