using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using D2D.UI;
using D2D.Utilities;

namespace D2D
{
    public abstract class FinishScreenSceneLoader : DButtonListener
    {
        [SerializeField] private bool _isButton;

        protected bool Was { get; private set; }

        private bool _isClaimClickMissed;
        
        private void Update()
        {
            if (!_isButton && DInput.IsMouseReleased)
                OnClick();
        }
        
        protected override void OnClick()
        {
            if (Was)
                return;

            Was = true;

            StartCoroutine(SceneReload());
        }

        private IEnumerator SceneReload()
        {
            // yield return null;

            var claimer = FindObjectOfType<ClaimRewardOnClick>();
            if (claimer != null && claimer.IsClaimed && !_isClaimClickMissed)
            {
                _isClaimClickMissed = true;
                Was = false;
                yield break;
            }
            
            LoadScene();
        }

        protected abstract void LoadScene();
    }
}