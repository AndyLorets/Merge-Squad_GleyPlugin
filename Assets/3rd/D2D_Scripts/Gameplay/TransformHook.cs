using System;
using D2D.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D2D.Common
{
    /// <summary>
    /// Now useful for a runner camera follow!
    /// </summary>
    public class TransformHook : MonoBehaviour
    {
        [TabGroup("Common")]
        [Tooltip("Hook target")]
        [SerializeField] private Transform target;
        
        [TabGroup("Common")]
        [Tooltip("Some nice follow smoothness (useful for cameras)")]
        [SerializeField] protected float followSmoothness = -1; // = .3f
        
        [TabGroup("Common")]
        [Tooltip("Do you wanna to copy parents rotation?")]
        public bool hookRotation = false;
        
        [TabGroup("Common")]
        [Tooltip("Do you wanna to set parent to null on start?")]
        public bool deattachParentOnStart = true;
        
        [TabGroup("Other")]
        [Tooltip("Do you wanna to follow from target center or with your origin?")]
        public bool useInitialSwift = true;
        
        [TabGroup("Other")]
        [Tooltip("Should it be destroy on parent destroy?")]
        public bool destroyOnParentNull = true;

        [TabGroup("Other")]
        [Tooltip("Is this script used in editor? To use OnDrawGizmo")]
        [SerializeField] private bool _isEditor;
        
        [TabGroup("Other")]
        [Tooltip("For ignoring certain axes use 0")]
        [SerializeField] private Vector3 _followAxes = Vector3.one;
        
        [TabGroup("Other")]
        [Tooltip("Sometimes can be useful LateUpdate for camera follow")]
        [SerializeField] private UpdateType _updateType;

        public bool HasTarget => target != null;

        public Transform Target
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                
                target = value;
                
                if (deattachParentOnStart)
                    transform.parent = null;
                
                if (useInitialSwift)
                    _initialSwift = transform.position - target.position;
            }
        }

        private Vector3 _initialSwift;
        private Vector3 _followVelocity;

        private void OnDrawGizmosSelected()
        {
            if (target != null && _isEditor)
            {
                var p = transform.position;
                if (_followAxes.x.Almost(1))
                {
                    p.x = target.position.x;
                }
                if (_followAxes.y.Almost(1))
                {
                    p.y = target.position.y;
                }
                if (_followAxes.z.Almost(1))
                {
                    p.z = target.position.z;
                }
                transform.position = p;
            }
        }

        protected virtual void Start()
        {
            Target = target;
        }

        private void Update()
        {
            if (_updateType == UpdateType.Update)
                Hook();
        }

        private void LateUpdate()
        {
            if (_updateType == UpdateType.LateUpdate)
                Hook();
        }

        private void FixedUpdate()
        {
            if (_updateType == UpdateType.FixedUpdate)
                Hook();
        }

        private void Hook()
        {
            if (!enabled)
                return;

            if (target == null)
            {
                if (destroyOnParentNull)
                    Destroy(gameObject);

                return;
            }
            
            var desired = target.position + _initialSwift;
            var p = transform.position;
            if (_followAxes.x.Almost(1))
                p.x = desired.x;
            if (_followAxes.y.Almost(1))
                p.y = desired.y;
            if (_followAxes.z.Almost(1))
                p.z = desired.z;

            if (followSmoothness >= 0)
                transform.position = Vector3.SmoothDamp(transform.position, p, ref
                    _followVelocity, followSmoothness);
            else
                transform.position = p;

            if (hookRotation)
                transform.rotation = target.rotation;
        }
    }
}