using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class LambdaState : AIState
    {
        private Action _onEnter = () => { }; 
        private Action _tick = () => { }; 
        private Action _onExit = () => { };

        public LambdaState(Action onEnter, Action tick, Action onExit, string commentary = "")
        {
            _onEnter = onEnter;
            _tick = tick;
            _onExit = onExit;

            Commentary = commentary;
        }
        
        public LambdaState(Action tick, string commentary = "")
        {
            _tick = tick;

            Commentary = commentary;
        }
        
        public override void OnEnter() => _onEnter();

        public override void Tick() => _tick();

        public override void OnExit() => _onExit();
    }
}