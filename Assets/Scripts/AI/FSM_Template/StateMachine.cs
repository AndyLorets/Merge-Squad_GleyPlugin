using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

// Notes
// 1. What a finite state machine is
// 2. Examples where you'd use one
//     AI, Animation, Game State
// 3. Parts of a State Machine
//     States & Transitions
// 4. States - 3 Parts
//     Tick - Why it's not Update()
//     OnEnter / OnExit (setup & cleanup)
// 5. Transitions
//     Separated from states so they can be re-used
//     Easy transitions from any state

public class StateMachine
{
   private class Transition
   {
      public Func<bool> Condition {get; }
      public AIState To { get; }
      public AIState From { get; }

      public Transition(AIState from, AIState to, Func<bool> condition)
      {
         From = from;
         To = to;
         Condition = condition;
      }
   }
   
   private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);
   
   private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
   private readonly List<Transition> _anyTransitions = new List<Transition>();
   
   private List<Transition> _currentTransitions = new List<Transition>();

   public AIState CurrentState { get; private set; }
   
   // 0 = no decision delay
   private float _decisionDelay = 0;
   
   private float _timeOfNextPossibleDecision;

   public StateMachine(float decisionDelay = 0)
   {
      _decisionDelay = decisionDelay;
   }

   public void Tick()
   {
      Transition transition = GetTransition();
      if (transition != null)
         SetState(transition.To);
      
      CurrentState?.Tick();
   }

   public void SetState(AIState newState)
   {
      if (newState == CurrentState)
         return;

      if (_decisionDelay > 0)
      {
         // Wait for next possible decision time
         if (Time.time < _timeOfNextPossibleDecision)
            return;
         
         _timeOfNextPossibleDecision = Time.time + _decisionDelay;
      }

      CurrentState?.OnExit();
      
      CurrentState = newState;
      
      _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
      if (_currentTransitions == null)
         _currentTransitions = EmptyTransitions;

      CurrentState.OnEnter();
   }

   public void AddTransition(AIState from, AIState to, Func<bool> predicate)
   {
      if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
      {
         transitions = new List<Transition>();
         _transitions[from.GetType()] = transitions;
      }

      transitions.Add(new Transition(from, to, predicate));
   }

   public void AddAnyTransition(AIState state, Func<bool> predicate)
   {
      _anyTransitions.Add(new Transition(null, state, predicate));
   }

   private Transition GetTransition()
   {
      foreach (Transition transition in _anyTransitions)
      {
         if (transition.Condition()) 
            return transition;
      }

      foreach (Transition transition in _currentTransitions)
      {
         if (transition.Condition() && transition.From == CurrentState) 
            return transition;
      }

      return null;
   }
}