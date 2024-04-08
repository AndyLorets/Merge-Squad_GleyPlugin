using D2D.Core;
using D2D.Utilities;
using UnityEngine;

namespace D2D.Cameras
{
    /// <summary>
    /// You can extend this class to any other game play shakes.
    /// Such as bomb explosion, enemy hit, etc.
    /// </summary>
    public class GameplayCameraShaker : CameraShaker
    {
        [SerializeField] private ShakeProfile _winShake;
        [SerializeField] private ShakeProfile _loseShake;
        
        private GameStateMachine _gsm;

        private void OnEnable()
        {
            _gsm = this.FindLazy<GameStateMachine>();

            _gsm.On<WinState>(() => Shake(_winShake));
            _gsm.On<LoseState>(() => Shake(_loseShake));
        }
    }
}