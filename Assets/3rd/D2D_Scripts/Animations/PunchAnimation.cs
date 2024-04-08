using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Animations;
using D2D.Gameplay;
using D2D.Utilities;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class PunchAnimation : DAnimation
    {
        [SerializeField] private bool _isPunchUI;
        [SerializeField] private int _vibratio;
        [SerializeField] private float _elasity;

        private Vector3 OriginalScale
        {
            get
            {
                _originalScale ??= transform.localScale;
                return _originalScale.Value;
            }
        }

        private Vector3? _originalScale;
        private Tween _lastPunch;

        protected override Tween CreateTween()
        {
            if (_lastPunch != null)
                _lastPunch.Kill();
            
            transform.localScale = OriginalScale;
            
            _lastPunch = _isPunchUI ? 
                Target.PunchUI() : 
                Target.DOPunchScale(Vector3.one * CalculatedTo, CalculatedDuration, _vibratio, _elasity);

            return _lastPunch;
        }
    }
}