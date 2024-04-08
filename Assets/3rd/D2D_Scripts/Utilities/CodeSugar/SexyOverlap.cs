using System;
using System.Collections.Generic;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    /// <summary>
    /// Forget about rigidbody bullshit and headache. Enjoy Sexy. Overlap...
    /// </summary>
    public class SexyOverlap : SmartScript
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private int _framesSkipBetween = 2;
        [SerializeField] private int _collidersCapacity = 100;
        [SerializeField] private bool _autoCastInUpdate = true;
        
        public Transform NearestTouched(Transform to) => HasTouch ? to.Closest(AllTouched).transform : null;
        public T NearestTouchedOfType<T>(Transform to) where T: Component => HasTouch ? to.ClosestOfType<T>(AllTouched) : null;
        public List<Collider> AllTouched { get; private set; }
        public Collider Touched { get; private set; }
        public bool HasTouch { get; private set; }

        private Collider[] _colliders;

        private bool _isSphere;
        private float _radius;
        private Vector3 _offset;

        private SphereCollider _sphere;

        private void OnDrawGizmos()
        {
            if (_isSphere)
            {
                Gizmos.color = Touched ? Color.green : Color.red;
                var p = transform.TransformPoint(_offset);
                var r = _radius * transform.localScale.x;
                Gizmos.DrawSphere(p, r);
            }
        }

        private void OnEnable()
        {
            _colliders = new Collider[_collidersCapacity];
            AllTouched = new List<Collider>();
            
            _sphere = Get<SphereCollider>();
            if (_sphere != null)
            {
                _isSphere = true;

                _radius = _sphere.radius;
                _offset = _sphere.center;

                _sphere.enabled = false;
            }
            
        }

        private void Start()
        {
            if (_sphere != null)
            {
                _sphere.enabled = false;
            }
        }

        private void Update()
        {
            if (!_autoCastInUpdate)
                return;

            if (Time.frameCount % _framesSkipBetween == 0)
                Cast();
        }

        public void Cast()
        {
            Array.Clear(_colliders,0, _collidersCapacity);
            
            var p = transform.TransformPoint(_offset);
            var r = _radius * transform.localScale.x;
            int numColliders = Physics.OverlapSphereNonAlloc(p, r, _colliders, _layer.value);
            
            HasTouch = false;
            Touched = null;

            AllTouched.Clear();
            
            for (var i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i] != null && _colliders[i].gameObject != gameObject)
                {
                    if (Touched == null)
                    {
                        Touched = _colliders[i];
                        HasTouch = true;
                    }
                    
                    AllTouched.Add(_colliders[i]);
                }
            }
        }
    }
}