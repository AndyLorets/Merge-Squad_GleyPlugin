/*using D2D.AI.Ghost;
using D2D.Gameplay;
using D2D.Utilities;
using UnityEngine;

namespace D2D.AI.Hunter
{
    public class HuntersAI : AIBase
    {
        [SerializeField] private HuntersAISettings _settings;
        [SerializeField] private Gameplay.Hunter _hunter;

        private CharacterController _cc;
        
        private void Start()
        {
            _hasher = this.FindLazy<UnitHub>();
            _fsm = new StateMachine();
            _hunter = GetComponent<Gameplay.Hunter>();
            _cc = GetComponent<CharacterController>();
            var map = FindObjectOfType<Map>();
            var runSpeed = _settings.movement.runSpeed;

            #region Transitions

            var wander = new WanderState(_settings.movement, transform, _cc, map);
            var runForGhost = new MoveTowardsTargetState(transform, _cc, GetNearestGhost, runSpeed, 1, "RunForGhost");
            var runForHunter = new MoveTowardsTargetState(transform, _cc, GetNearestEatableHunter, runSpeed, 1, "RunForHunter");
            var runAwayHunter = new MoveTowardsTargetState(transform, _cc, GetNearestDangerousHunter, runSpeed, -1, "RunAwayHunter");

            #endregion

            #region At

            At(wander, runForGhost, IsGhostNear);
            At(wander, runForHunter, IsHunterToEatNear);
            At(wander, runAwayHunter, IsDangerousHunterNear);
            
            At(runForGhost, wander, CanWander);
            At(runForGhost, runForHunter, IsHunterToEatNear);
            At(runForGhost, runAwayHunter, IsDangerousHunterNear);
            
            At(runForHunter, wander, CanWander);
            At(runForHunter, runAwayHunter, IsDangerousHunterNear);
            
            At(runAwayHunter, wander, CanWander);

            #endregion

            _fsm.SetState(wander);
        }

        protected override string GetAdditionalFsmDebugInfo()
        {
            return $"\nG:{GetNearestGhost()?.GetInstanceID()}" +
                   $"\nE:{GetNearestEatableHunter()?.GetInstanceID()}" +
                   $"\nIsGhostFar:{IsGhostFar()}" +
                   $"\nIsHunterToEatNear:{IsHunterToEatNear()}" +
                   $"\nIsDangerousHunterNear:{IsDangerousHunterNear()}" +
                   $"\nD:{GetNearestDangerousHunter()?.GetInstanceID()}";
        }

        private bool CanWander() => IsGhostFar() && IsDangerousHunterFar() && IsHunterToEatFar();

        private bool IsGhostFar()
            => IsOutOfVision(_hasher.Ghosts, _settings.runForGhost);
        
        private Transform GetNearestGhost()
            => GetNearest(_hasher.Ghosts, _settings.runForGhost.Distance, 
                g => _hunter.Level >= g.Level);

        private bool IsGhostNear()
            => GetNearestGhost() != null;


        private Transform GetNearestEatableHunter() 
            => GetNearest(_hasher.Hunters, _settings.runForHunter.Distance, 
                h => _hunter.Level >= h.Level + GameplaySettings.Instance.eatLevelDistance);

        private bool IsHunterToEatFar() 
            => IsOutOfVision(_hasher.Hunters, _settings.runForHunter);

        private bool IsHunterToEatNear()
            => GetNearestEatableHunter() != null;

        
        private Transform GetNearestDangerousHunter() 
            => GetNearest(_hasher.Hunters, _settings.runAwayHunter.Distance, 
                h => _hunter.Level <= h.Level - GameplaySettings.Instance.eatLevelDistance);

        private bool IsDangerousHunterFar() 
            => IsOutOfVision(_hasher.Hunters, _settings.runAwayHunter);
        
        private bool IsDangerousHunterNear()
            => GetNearestDangerousHunter() != null;
    }
}*/