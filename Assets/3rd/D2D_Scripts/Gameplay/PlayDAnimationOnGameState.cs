using Animancer;
using D2D.Animations;
using D2D.Core;
using D2D.Utilities;
using UnityEngine;

namespace _3rd.D2D_Scripts.Gameplay
{
    public class PlayDAnimationOnGameState : GameStateMachineUser
    {
        [SerializeField] private DAnimation[] _onRunning;
        [SerializeField] private DAnimation[] _onWin;
        [SerializeField] private DAnimation[] _onLose;
        [SerializeField] private DAnimation[] _onGameFinish;
        
        protected override void OnGameRun() => Play(_onRunning);
        
        protected override void OnGameWin() => Play(_onWin);
        
        protected override void OnGameLose() => Play(_onLose);
        
        protected override void OnGameFinish() => Play(_onGameFinish);

        private void Play(DAnimation[] anims)
        {
            if (anims.IsNullOrEmpty())
                return;
            
            anims.PlayAll();
        }
    }
}