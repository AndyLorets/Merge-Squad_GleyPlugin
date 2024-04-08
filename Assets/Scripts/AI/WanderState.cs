/*using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using AI;
using D2D;
using D2D.AI.Hunter;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utilities.CodeSugar;
using Scripts.Utilities;

namespace D2D.AI.Ghost
{
    public class WanderState : AIState
    {
        private readonly UnitMovementSettings _settings;
        private readonly Transform _transform;
        private readonly CharacterController _cc;
        private readonly Rigidbody _rb;
        private readonly Map _map;

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

        private float _angleChangeSpeed;
        private float _speed;
        private float _timeToNextFarAwayCheck;

        public WanderState(UnitMovementSettings settings, Transform transform, 
            CharacterController cc, Map map)
        {
            _settings = settings;
            _transform = transform;
            _cc = cc;
            _map = map;
        }
        
        public WanderState(UnitMovementSettings settings, Transform transform, 
            Rigidbody rb, Map map)
        {
            _settings = settings;
            _transform = transform;
            _rb = rb;
            _map = map;
        }

        public override void OnEnter()
        {
            var angle = DMath.Random(0, 360f);
            
            _transform.LocalEuler().Y = angle;

            _angleChangeSpeed = DMath.RandomByAmplitude(_settings.wanderAngleChangeSpeed);
            _speed = _settings.wanderSpeed.RandomFloat();
        }
        
        public override void Tick()
        {
            _transform.eulerAngles += new Vector3(0, _angleChangeSpeed) * Time.deltaTime;
            
            // Velocity = _transform.forward * _speed;
            _cc.SimpleMove(_transform.forward * _speed);

            if (Time.time > _timeToNextFarAwayCheck)
                CheckForFarAway();

            if (Velocity.magnitude < GeneralAISettings.Instance.stuckSpeed)
            {
                OnEnter();
            }
        }

        private void CheckForFarAway()
        {
            _timeToNextFarAwayCheck = Time.time + GeneralAISettings.Instance.farAwayCheckDelay;

            if (_map.IsInsideMap(_transform.position))
                return;

            var p = _map.Middle;
            p.y = _transform.position.y;
            _transform.LookAt(p);
        }

        public override void OnExit()
        {
            _cc.SimpleMove(Vector3.zero);
            // Velocity = Vector3.zero;
        }
    }
}*/