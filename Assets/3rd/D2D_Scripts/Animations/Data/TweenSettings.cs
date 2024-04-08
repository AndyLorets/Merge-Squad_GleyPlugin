using System;
using D2D.Utilities;
using D2D.Utils;
using DG.Tweening;
using UnityEngine;

namespace D2D.Animations
{
    [Serializable]
    public class TweenData
    {
        public float duration;
        public float amplitude;
        public Ease ease;
    }
    
    [Serializable]
    public class PunchData : TweenData
    {
        public int vibrato;
        public float elasticity;
    }
    
    [Serializable]
    public class ShakeData : TweenData
    {
        public int vibrato;
        public float randomness;
    }
    
    [CreateAssetMenu(fileName = "TweenSettings", menuName = "SO/TweenSettings")]
    public class TweenSettings : SingletonData<TweenSettings>
    {
        [Header("General")] 
        [Tooltip("To remove tweens safely set this value to ~ 0.05")] 
        public float killDelay = .05f;
        
        public PunchData UIPunch = new PunchData
        {
            duration = .3f, 
            amplitude = .2f,
            ease = Ease.InOutSine,
            vibrato = 8,
            elasticity = 1,
        };
        public PunchData GameplayPunch = new PunchData
        {
            duration = .2f, 
            amplitude = .2f,
            ease = Ease.OutQuad,
            vibrato = 10,
            elasticity = 1,
        };

        [Space(10)] 
        
        public ShakeData UIShake;
        public ShakeData GameplayShake;
    }
}