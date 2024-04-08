using System;
using UnityEngine;

namespace D2D.Utilities
{
    public static class Vector3ClassSugar
    {
        /// <summary>
        /// It is SLOW! Avoid using it for crowds of objects in Update()
        /// </summary>
        public class Vector3Class
        {
            private readonly Func<Vector3> _getter;
            private readonly Action<Vector3> _setter;

            public Vector3Class(Func<Vector3> getter, Action<Vector3> setter)
            {
                _getter = getter;
                _setter = setter;
            }

            public float x
            {
                get => _getter().x;
                set
                {
                    var v = _getter();
                    v.x = value;
                    _setter(v);
                }
            }
        
            public float y
            {
                get => _getter().y;
                set
                {
                    var v = _getter();
                    v.y = value;
                    _setter(v);
                }
            }
        
            public float z
            {
                get => _getter().z;
                set
                {
                    var v = _getter();
                    v.z = value;
                    _setter(v);
                }
            }
        }
        
        public static Vector3Class Position(this Transform target)
        {
            if (target == null)
                throw new NullReferenceException("target");
            
            return new Vector3Class(() => target.position, (v) => target.position = v);
        }
        
        public static Vector3Class Velocity(this Rigidbody target)
        {
            if (target == null)
                throw new NullReferenceException("target");
            
            return new Vector3Class(() => target.velocity, (v) => target.velocity = v);
        }
        
        public static Vector3Class LocalPosition(this Transform target)
        {
            if (target == null)
                throw new NullReferenceException("target");
            
            return new Vector3Class(() => target.localPosition, (v) => target.localPosition = v);
        }
        
        public static Vector3Class Euler(this Transform target)
        {
            if (target == null)
                throw new NullReferenceException("target");
            
            return new Vector3Class(() => target.eulerAngles, (v) => target.eulerAngles = v);
        }
        
        public static Vector3Class LocalEuler(this Transform target)
        {
            if (target == null)
                throw new NullReferenceException("target");
            
            return new Vector3Class(() => target.localEulerAngles, (v) => target.localEulerAngles = v);
        }
    }
}