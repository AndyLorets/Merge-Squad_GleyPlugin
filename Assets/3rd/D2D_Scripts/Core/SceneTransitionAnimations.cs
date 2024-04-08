using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Animations;
using D2D.Core;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    /// <summary>
    /// Should be as folder GO, inside will be two screens with DAnimations
    /// (kill on finish for fadeIn)
    /// Fade in will setup from start automatically.
    /// </summary>
    public class SceneTransitionAnimations : SmartScript
    {
        [SerializeField] private GameObject _fadeOut;

        private void Start()
        {
            _stateMachine.On<SceneLoading>(FadeOut);
        }

        private void FadeOut()
        {
            _fadeOut.On(_coreData.nextLevelLoadDelay);
        }
    }
}