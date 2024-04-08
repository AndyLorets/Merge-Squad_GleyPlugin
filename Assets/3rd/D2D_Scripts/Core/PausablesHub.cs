using System.Collections.Generic;
using D2D.Utilities;
using D2D.Tools;
using UnityEngine;

namespace D2D.Core
{
    /// <summary>
    /// Activates and deactivates pausables according to
    /// current game state IsGameActiveDuringState option
    /// </summary>
    public class PausablesHub : MonoBehaviour, ILazy
    {
        /// <summary>
        /// All existed pausable objects in game 
        /// </summary>
        private static List<PausablesMember> _pausables = new List<PausablesMember>();
        
        private GameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _gameStateMachine = this.FindLazy<GameStateMachine>();
            
            // Adjust switching pausable activity to any state
            _gameStateMachine.On<GameState>(UpdatePausablesActivityAccordingToState, gameObject);
        }
        
        private void UpdatePausablesActivityAccordingToState()
        {
            for (int i = 0; i < _pausables.Count; i++)
            {
                // Remove null or already destroyed pausables
                if (_pausables[i] == null || _pausables[i].gameObject == null)
                {
                    _pausables.RemoveAt(i);
                    continue;
                }
                
                // If all is ok, update pausable activity according to current game state
                // _pausables[i].gameObject.SetActive(_gameStateMachine.Last.IsGameActiveDuringState);
            }
        }

        /// <summary>
        /// Register pausable. So now, new object will be paused or not according to game state
        /// </summary>
        public void AddPausable(PausablesMember newPausable)
        {
            _pausables.Add(newPausable);

            // if (_gameStateMachine.IsEmpty || !_gameStateMachine.Last.IsGameActiveDuringState)
               // newPausable.gameObject.SetActive(false);
        }
    }
}
