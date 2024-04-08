/*using UnityEngine;
using D2D.Gameplay;
using D2D.Utilities;

namespace D2D.AI.Ghost
{
    public class GhostRunAway : AIState
    {
        private readonly GhostAISettings _settings;
        private readonly Transform _transform;
        private readonly CharacterController _characterController;
        private readonly Transform _raycastPoint;
        
        private float _speed;
        private bool _needRunAway;
        
        public GhostRunAway(GhostAISettings settings, Transform transform, CharacterController characterController, Transform raycastPoint)
        {
            _settings = settings;
            _transform = transform;
            _characterController = characterController;
            _raycastPoint = raycastPoint;
        }
    
        public override void OnEnter()
        {
            var hits = _raycastPoint.GetHitAngular(_transform.forward, 
                _settings.nearHunter.Distance, GameplaySettings.Instance.huntersLayer, 20);

            // _needRunAway = !hits.IsNullOrEmpty();

            var direction = Vector3.zero;
            foreach (var hit in hits)
            {
                direction += hit.Value.normal;
            }
            direction /= hits.Length;
            direction.y = 0;

            _transform.forward = direction;
            
            if (hits.IsNullOrEmpty())
                _transform.LocalEuler().Y = DMath.Random(0, 360);
            
            _speed = _settings.movement.runSpeed.RandomFloat();
        }
        
        public override void Tick()
        {
            _characterController.SimpleMove(_transform.forward * _speed);
        }

        public override void OnExit()
        {
            _characterController.SimpleMove(Vector3.zero);
        }
    }
}*/