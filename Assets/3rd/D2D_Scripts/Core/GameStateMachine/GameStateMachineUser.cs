using System;
using D2D.Utilities;
using D2D.Tools;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Core
{
    /// <summary>
    /// Just to avoid same code writing. So many scripts usually hashes GSM, so here is a.
    /// hasher template for them.
    /// </summary>
    public class GameStateMachineUser : SmartScript
    {
        protected virtual void OnEnable()
        {
            BindCallbacks();
        }

        protected virtual void OnDisable()
        {
            if (_coreData.stateMachineOptimizedMode)
                _stateMachine.RemoveSubscriber(gameObject);
        }

        private void BindCallbacks()
        {
            On<RunningState>(OnGameRun);
            On<PauseState>(OnGamePause);
            On<WinState>(OnGameWin);
            On<LoseState>(OnGameLose);
            On<SceneLoading>(OnPostgame);
            
            On<WinState, LoseState>(OnGameFinish);
            
            On<CustomStateA>(OnCustomStateA);
            On<CustomStateB>(OnCustomStateB);
            On<CustomStateC>(OnCustomStateC);
        }

        protected virtual void OnCustomStateA() { }
        protected virtual void OnCustomStateB() { }
        protected virtual void OnCustomStateC() { }

        protected virtual void OnGameRun() { }

        protected virtual void OnGamePause() { }
        
        protected virtual void OnGameWin() { }
        
        protected virtual void OnGameLose() { }
        
        protected virtual void OnPostgame() { }

        protected virtual void OnGameFinish() { }

        protected void On<TState>(Action a) where TState : GameState
        {
            _stateMachine.On<TState>(a, gameObject);
        }
        
        protected void On<TState1, TState2>(Action a) 
            where TState1 : GameState
            where TState2 : GameState
        {
            _stateMachine.On<TState1, TState2>(a, gameObject);
        }
    }
}