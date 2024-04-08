using D2D.Animations;
using DG.Tweening;
using UnityEngine;

namespace D2D.Utilities
{
    public static class TweenSugar
    {
        public static void RestartTween(this GameObject target, string id)
        {
            target.GetComponent<DOTweenAnimation>().DORestartAllById(id);
        }

        private static Tweener Punch(this Transform target, PunchData data)
        {
            return target.DOPunchScale(data.amplitude * Vector3.one,
                data.duration, data.vibrato, data.elasticity).SetEase(data.ease);
        }

        public static Tweener PunchUI(this Transform target)
        {
            return Punch(target, TweenSettings.Instance.UIPunch);
        }
        
        public static Tweener PunchGameplay(this Transform target)
        {
            return Punch(target, TweenSettings.Instance.GameplayPunch);
        }
        
        private static Tweener Shake(this Transform target, ShakeData data)
        {
            return target.DOShakePosition(data.duration, data.amplitude, 
                data.vibrato, data.randomness).SetEase(data.ease);
        }

        public static Tweener ShakeUI(this Transform target)
        {
            return Shake(target, TweenSettings.Instance.UIShake);
        }
        
        public static Tweener ShakeGameplay(this Transform target)
        {
            return Shake(target, TweenSettings.Instance.GameplayShake);
        }
    }
}