using System;
using D2D.Animations;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class TweenCheck : SmartScript
    {
        [SerializeField] private DAnimation _positive;
        [SerializeField] private DAnimation _negative;

        private void OnEnable()
        {
            _positive.Tick += () => Debug.Log("...");
        }

        private void Update()
        {
            if (DInput.IsUpPressed)
            {
                // if (!_positive.IsPlaying)
                {
                    _negative.Kill();
                    _positive.Kill();
                    _positive.Restart();
                }
            }
            
            if (DInput.IsDownPressed)
            {
                // if (!_negative.IsPlaying)
                {
                    _negative.Kill();
                    _positive.Kill();
                    _negative.Restart();
                }
            }
        }
    }
}