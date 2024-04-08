using System;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using UnityEngine;
using D2D.Tools;
using D2D.Utils;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Core
{
    public enum CommonGameState
    {
        None, 
        Running,
        Menu
    }
    
    /// <summary>
    /// Owner and its action. Used to unsub zero actions with already destroyed owners
    /// </summary>
    public class Subscriber
    {
        public GameObject owner;
        public Action action;
    }
    
    /// <summary>
    /// Powerful and safe game state machine which extensible (use classes instead of fixed enums)
    /// and allows to check is next state possible, count happened states
    ///
    /// In fact we could do game states as SO. But, it won`t be so useful in code.
    /// </summary>
    public class GameStateMachine : MonoBehaviour, ILazy
    {
        [SerializeField] private CommonGameState _startState;
        
        public bool IsEmpty => _states.Count == 0;
        
        public bool IsGameFinished => WasAny<WinState, LoseState>();

        public bool WasWin => Was<WinState>();
        
        public bool WasLose => Was<LoseState>();
        
        public GameState Last { get; private set; }
        
        // Should be used only by editor
        public IEnumerable<GameState> StatesForEditor => _states;
        
        private List<GameState> _states = new List<GameState>();

        /// <summary>
        /// For each game state we have a list of subscribers' actions 
        /// </summary>
        private Dictionary<Type, List<Subscriber>> _subscribersMap = 
            new Dictionary<Type, List<Subscriber>>();
        
        private Dictionary<Type, GameState> _happenedStates = 
            new Dictionary<Type, GameState>();

        private void Start()
        {
            if (_startState == CommonGameState.Running)
            {
                Push(new RunningState());
            }
            else if (_startState == CommonGameState.Menu)
            {
                Push(new PauseState());
            }
        }

        /// <summary>
        /// Attach some action to some game state
        /// </summary>
        public void On<TState>(Action action, GameObject owner = null) where TState : GameState
        {
            // Init dictionary if needed
            if (!_subscribersMap.ContainsKey(typeof(TState)))
                _subscribersMap[typeof(TState)] = new List<Subscriber>();
            
            // Add new subscriber
            var newSubscriber = new Subscriber
            {
                owner = owner,
                action = action,
            };
            
            _subscribersMap[typeof(TState)].Add(newSubscriber);
        }
        
        /// <summary>
        /// On with 2 game state binding.
        /// </summary>
        public void On<TState1, TState2>(Action action, GameObject owner = null) 
            where TState1 : GameState
            where TState2 : GameState
        {
            On<TState1>(action, owner);
            On<TState2>(action, owner);
        }
        
        /// <summary>
        /// On with 3 game state binding.
        /// </summary>
        public void On<TState1, TState2, TState3>(Action action, GameObject owner = null) 
            where TState1 : GameState
            where TState2 : GameState
            where TState3 : GameState
        {
            On<TState1>(action, owner);
            On<TState2>(action, owner);
            On<TState3>(action, owner);
        }

        /// <summary>
        /// Push new game state and notify others of it.
        /// </summary>
        public void Push(GameState newState, bool safely = false)
        {
            if (!IsStatePossible(newState))
            {
                if (!safely)
                    Debug.LogError($"{newState} can't go after {Last}");
                
                return;
            }

            _happenedStates[newState.GetType()] = newState;
            _states.Add(newState);
            Last = newState;
            
            newState.Action();
            
            NotifySubscribers(newState.GetType());
            
            // For subs who are listening for every state
            // Emit(typeof(GameState));
        }
        
        public void Push<TState>(bool safely = false) where TState : GameState
        {
            _happenedStates.TryGetValue(typeof(TState), out GameState g);
            g ??= (TState)Activator.CreateInstance(typeof(TState));

            Push(g, safely);
        }

        /// <summary>
        /// Can new state go after last state?
        /// #important
        /// </summary>
        private bool IsStatePossible(GameState newState)
        {
            if (Last == null)
                return true;

            // State should be different
            if (!newState.CanRepeat && Last.GetType() == newState.GetType())
                return false;
            
            // Any new state after last scene loading??? Nee, it is not possible
            if (Last.Is<SceneLoading>())
                return false;

            if (Last.Is<LoseState>() && newState.Is<WinState>())
                return false;
            
            if (Last.Is<WinState>() && newState.Is<LoseState>())
                return false;

            return true;
        }

        /// <summary>
        /// Notify others of a new happened game state.
        /// </summary>
        private void NotifySubscribers(Type t)
        {
            if (!_subscribersMap.TryGetValue(t, out var subs))
                return;
            
            if (subs.IsNullOrEmpty())
                return;
            
            // For here cuz we will use remove at
            for (int i = 0; i < subs.Count; i++)
            {
                // Remove subscriber if it gameObject or action isn`t exists anymore 
                if (subs[i].action == null)
                {
                    if (_coreData.stateMachineOptimizedMode ||
                        (_coreData.stateMachineOptimizedMode && subs[i].owner == null))
                    {
                        subs.RemoveAt(i);
                    }
                }
                // If all is ok, execute subscriber`s action 
                else
                {
                    if (_coreData.stateMachineOptimizedMode)
                    {
                        subs[i].action?.Invoke();
                    }
                    else
                    {
                        try
                        {
                            subs[i].action?.Invoke();
                        }
                        catch (Exception e)
                        {
                            // Died member action... skip.
                        }
                    }
                }
            }
        }

        public void RemoveSubscriber(GameObject removingOwner)
        {
            if (!_coreData.stateMachineOptimizedMode)
                return;

            Debug.Log($"Remove: {removingOwner}");

            if (removingOwner == null)
                return;
            
            foreach (var typeSubscribersPair in _subscribersMap)
            {
                var subs = typeSubscribersPair.Value;
                
                if (subs.IsNullOrEmpty())
                    continue;

                for (var i = 0; i < subs.Count; i++)
                {
                    if (subs[i].owner == removingOwner)
                        subs.RemoveAt(i);
                }
            }
        }

        public GameState GetLastOfType<TState>() where TState : GameState
        {
            _happenedStates.TryGetValue(typeof(TState), out GameState s);
            return s;
        }

        /// <summary>
        /// How many there were states of this type?
        /// </summary>
        public int Count<TState>() where TState : GameState
        {
            return _states.Count(s => s.GetType() == typeof(TState));
        }

        /// <summary>
        /// Was this state emitted?
        /// </summary>
        public bool Was<TState>() 
            where TState : GameState
        {
            return _happenedStates.ContainsKey(typeof(TState));
        }
        
        /// <summary>
        /// Was any of these states emitted?
        /// </summary>
        public bool WasAny<TState1, TState2>()
            where TState1 : GameState
            where TState2 : GameState
        {
            return Was<TState1>() || Was<TState2>();
        }
    }
}