/*
using System.Linq;
using D2D.Utilities;
using UnityEngine;

namespace D2D.AI.Ghost
{
    public class GhostAI : AIBase
    {
        [SerializeField] private GhostAISettings _settings;
        [SerializeField] private Transform _raycastPoint;

        private Transform _nearestHunter;
        private bool _isHunterFar;

        private CharacterController _cc;

        protected override float TickDelay => _settings.observeDelay;

        private void Start()
        {
            _hasher = this.FindLazy<UnitHub>();
            _fsm = new StateMachine();
        
            _cc = GetComponent<CharacterController>();
            var map = FindObjectOfType<Map>();
            
            var wander = new WanderState(_settings.movement, transform, _cc, map);
            var runAway = new GhostRunAway(_settings, transform, _cc, _raycastPoint);
        
            At(wander, runAway, IsHunterNear);
            At(runAway, wander, IsHunterFarVariable);

            _fsm.SetState(wander);
        }

        private bool IsHunterNear() 
            => _nearestHunter != null;

        private Transform GetNearestHunter()
            => GetNearest(_hasher.Hunters, _settings.nearHunter.Distance, h => true);

        private bool IsHunterFarVariable() 
            => _isHunterFar;

        private bool IsHunterFar() 
            => _hasher.Hunters.All(h => 
                _settings.farHunter.IsOutOfVision(h.transform, transform));

        protected override void Tick()
        {
            _nearestHunter = GetNearestHunter();
            _isHunterFar = IsHunterFar();
        }
    }
}
*/
