using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;

namespace D2D.AI.Hunter
{
    public class MoveTowardsTargetState : AIState
    {
        private readonly Transform _transform;
        private readonly CharacterController _cc;
        private readonly Rigidbody _rb;
        private readonly Func<Transform> _getNearestTarget;
        private readonly Vector2 _runSpeedRange;
        private readonly int _direction;

        protected Vector3 Velocity
        {
            get => _cc != null ? _cc.velocity : _rb.velocity;
            set
            {
                if (_cc != null)
                    _cc.SimpleMove(value);
                else
                    _rb.velocity = value;
            }
        }
        
        private Transform _target;
        
        private float _runSpeed;

        public MoveTowardsTargetState(Transform transform, CharacterController cc, 
            Func<Transform> getNearestTarget, Vector2 runSpeedRange, int direction, string commentary)
        {
            _transform = transform;
            _cc = cc;
            _getNearestTarget = getNearestTarget;
            _runSpeedRange = runSpeedRange;
            _direction = direction;
            Commentary = commentary;
        }
        
        public MoveTowardsTargetState(Transform transform, Rigidbody rb, 
            Func<Transform> getNearestTarget, Vector2 runSpeedRange, int direction = 1)
        {
            _transform = transform;
            _rb = rb;
            _getNearestTarget = getNearestTarget;
            _runSpeedRange = runSpeedRange;
            _direction = direction;
        }
    
        public override void OnEnter()
        {
            _target = _getNearestTarget();
            _runSpeed = _runSpeedRange.RandomFloat();
        }

        private void FaceToEnemy()
        {
            var d = _transform.position - _target.position;

            if (ReferenceEquals(_transform, _target) || d.magnitude.AlmostZero(.01f))
                throw new Exception("Face to yourself!");
            
            d.y = 0;
            _transform.forward = -d * _direction;
        }

        public override void Tick()
        {
            if (_target == null)
            {
                OnEnter();

                if (_target == null)
                    return;
            }
            
            FaceToEnemy();

            _cc.SimpleMove(_transform.forward * _runSpeed);
            // Velocity = _transform.forward * _runSpeed;
        }

        public override void OnExit()
        {
            _cc.SimpleMove(Vector3.zero);
            // Velocity = Vector3.zero;
        }
    }
}