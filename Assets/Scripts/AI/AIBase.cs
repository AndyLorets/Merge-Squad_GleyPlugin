using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using TMPro;

namespace D2D
{
    public abstract class AIBase : GameStateMachineUser
    {
        // If you want to make decisions not every update => type the delay in seconds
        [SerializeField] private float _tickDelay = 1;
        
        protected StateMachine _fsm;
        protected UnitHasher _hasher;

        private TMP_Text _stateLabel;
        private float _timeOfNextTick;

        
        protected virtual void Awake()
        {
            if (GeneralAISettings.Instance.fsmDebugEnabled)
                CreateStateLabel();

            _hasher = this.FindLazy<UnitHasher>();
            InitStateMachine();
        }

        protected abstract void InitStateMachine();

        private void CreateStateLabel()
        {
            var prefab = GeneralAISettings.Instance.debugCanvasPrefab;
            var newCanvas = Instantiate(prefab, transform);
            newCanvas.transform.localPosition = Vector3.zero;

            _stateLabel = newCanvas.GetComponentInChildren<TMP_Text>();
        }

        protected void At(AIState a, AIState b, Func<bool> condition) 
            => _fsm.AddTransition(a, b, condition);
            
        // protected void To(AIState a, AIState b, Func<Transform> condition) 
        //     => _stateMachine.AddTransition(a, b, () => condition() != null);
        

        /*protected bool IsOutOfVision(IEnumerable<Unit> units, VisionZone visionZone) 
            => units
                .All(unit => ReferenceEquals(unit.gameObject, gameObject) || 
                             visionZone.IsOutOfVision(unit.transform, transform));*/
        
        // private bool IsInVision(IEnumerable<Unit> units, VisionZone visionZone, Func<Unit, bool> predicate) =>
        //     units
        //         .FirstOrDefault(g => 
        //             visionZone.IsInVision(g.transform, transform) && predicate(g));

        protected Transform GetNearest(IEnumerable<Unit> units, float d, Func<Unit, bool> predicate) 
            => units
                .FirstOrDefault(unit => 
                    !ReferenceEquals(unit.gameObject, gameObject) && 
                    (transform.position - unit.transform.position).sqrMagnitude < d * d &&
                    predicate(unit))
                ?.transform;

        private void Update()
        {
            if (!enabled || !GeneralAISettings.Instance.isAIEnabled)
                return;
            
            _fsm.Tick();

            if (GeneralAISettings.Instance.fsmDebugEnabled)
                DisplayFsmInfo();
            
            if (_tickDelay < 0)
            {
                Tick();
            }
            else
            {
                if (Time.time > _timeOfNextTick)
                {
                    Tick();
                    _timeOfNextTick = Time.time + _tickDelay;
                }
            }
        }

        private void DisplayFsmInfo()
        {
            var basicStateInfo = _fsm.CurrentState.Commentary;
                
            if (basicStateInfo == "")
                basicStateInfo = _fsm.CurrentState.GetType().FullName.Split('.').Last();
            
            _stateLabel.text = basicStateInfo + GetAdditionalFsmDebugInfo();
        }

        protected virtual string GetAdditionalFsmDebugInfo()
        {
            return "";
        }

        protected virtual void Tick()
        {
            
        }
        
        protected override void OnGameFinish()
        {
            enabled = false;
        }
    }
}