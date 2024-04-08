using Cysharp.Threading.Tasks;
using D2D.Gameplay;
using DG.Tweening;
using UnityEngine;

namespace D2D.Utilities
{
    /// <summary>
    /// For time management in game and especially for slow-mos.
    /// </summary>
    public static class DTime
    {
        private const float FixedDeltaTimeFactor = .02f;
        private const float SlowMoTimeScale = .2f;
        private const float SlowMoDuration = 1.5f;
        private const float SlowMoLerpDuration = .3f;
        
        public static float TimeScale
        {
            get => Time.timeScale;
            set
            {
                Time.timeScale = value;
                Time.fixedDeltaTime = value * FixedDeltaTimeFactor;
            }
        }

        public static async UniTaskVoid SlowMo()
        {
            DOVirtual.Float(1f, SlowMoTimeScale, SlowMoLerpDuration, x => TimeScale = x);
            await UniTask.Delay((SlowMoDuration * 1000).Round(), true);
            DOVirtual.Float(SlowMoTimeScale, 1f , SlowMoLerpDuration, x => TimeScale = x);
        }
    }
}