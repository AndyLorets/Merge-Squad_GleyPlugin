using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class FlyingUIIcon : SmartScript
    {
        private Tween _punch;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void Punch()
        {
            _punch?.Pause();
            _punch?.Kill();

            transform.localScale = _originalScale;
            _punch = transform.PunchUI();
        }
    }
}