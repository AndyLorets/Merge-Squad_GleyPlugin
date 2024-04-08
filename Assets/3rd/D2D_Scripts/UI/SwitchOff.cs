using UnityEngine;
using D2D.Core;
using D2D.Utilities;


namespace D2D
{
    public class SwitchOff : GameStateMachineUser
    {
        private enum GameState
        {
            Running,
            Win,
            Lose,
            GameFinish,
        }
        
        [SerializeField] private GameState _on;

        protected override void OnGameRun() => DisableIf(GameState.Running);

        protected override void OnGameFinish() => DisableIf(GameState.GameFinish);

        protected override void OnGameLose() => DisableIf(GameState.Lose);

        protected override void OnGameWin() => DisableIf(GameState.Win);

        private void DisableIf(GameState s)
        {
            if (_on == s)
                gameObject.Off();
        }
    }
}