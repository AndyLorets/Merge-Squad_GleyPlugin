using System;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using UnityEngine;

namespace D2D
{
    public class Finish : OnceObjectInteractor<Player>
    {
        [SerializeField] private GameObject _activateOnWin;

        protected override void OnInteract(Player player)
        {
            this.FindLazy<GameStateMachine>().Push(new WinState());
            
            // Confetti, win camera, etc.
            _activateOnWin.On();
        }
    }
}