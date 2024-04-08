using System;
using DG.Tweening;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    public abstract class AnimationSettingsBase
    {
        public abstract Tween Play(Transform target);
    }
    
    [Serializable]
    public class Move : AnimationSettingsBase
    {
        public Vector3 to;
        public float duration = 1;
        public Ease ease = Ease.Linear;
        public bool isRelative = true;
        
        public override Tween Play(Transform target)
        {
            return target.DOMove(to, duration).SetEase(ease).SetRelative(isRelative);
        }
    }
    
    [Serializable]
    public class Scale : AnimationSettingsBase
    {
        public float to = 1;
        public float duration = 1; 
        public Ease ease = Ease.Linear;
        public bool isRelative;
        
        public override Tween Play(Transform target)
        {
            return target.DOScale(to, duration).SetEase(ease).SetRelative(isRelative);
        }
    }
    
    [Serializable]
    public class Rotation : AnimationSettingsBase
    {
        public Vector3 to;
        public float duration = 1; 
        public Ease ease = Ease.Linear;
        public bool isRelative;
        public bool isLocal;
        
        public override Tween Play(Transform target)
        {
            return isLocal ? target.DOLocalRotate(to, duration).SetEase(ease).SetRelative(isRelative) : 
                target.DORotate(to, duration).SetEase(ease).SetRelative(isRelative);
        }
    }
}