using D2D.Core;
using UnityEngine;

namespace D2D
{
    public class DestroyOnPostgame : GameStateMachineUser
    {
        [SerializeField] private float _delay;
        
        protected override void OnGameWin()
        {
            Destroy(gameObject, _delay);
        }

        protected override void OnGameLose()
        {
            Destroy(gameObject, _delay);
        }
    }
}