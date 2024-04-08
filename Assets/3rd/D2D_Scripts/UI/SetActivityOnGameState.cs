using UnityEngine;
using D2D.Core;
using D2D.Utilities;

namespace D2D
{
    public class SetActivityOnGameState : GameStateMachineUser
    {
        [SerializeField] private bool _toggleToActive = true;
        [SerializeField] private float _delay;
        
        public GameObject[] _onRunning;
        public GameObject[] _onWin;
        public GameObject[] _onLose;
        public GameObject[] _onGameFinish;

        protected override void OnGameRun() => Show(_onRunning);

        protected override void OnGameFinish() => Show(_onGameFinish);

        protected override void OnGameLose() => Show(_onLose);

        protected override void OnGameWin() => Show(_onWin);

        private void Show(GameObject[] parent)
        {
            if (parent.IsNullOrEmpty()) 
                return;
            
            foreach (GameObject o in parent)
            {
                float delay = _delay;
                
                if (o == null)
                    continue;

                var window = o.GetComponent<Window>();

                if (window != null)
                    delay = window.OpenDelay;
                    
                if (_toggleToActive)
                    o.On(delay);
                else
                    o.Off(delay);
            }
        }
    }
}