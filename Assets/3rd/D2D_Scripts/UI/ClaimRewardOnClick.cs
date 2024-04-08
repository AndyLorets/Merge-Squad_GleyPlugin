using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.UI;
using D2D.Utilities;
using D2D.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class ClaimRewardOnClick : DButtonListener
    {
        [SerializeField] private bool _isButton;

        [Header("Animations")] 
        [SerializeField] private bool _isBunchEffect;
        [SerializeField] private bool _showClaimAnimation;
        [SerializeField] private bool _showBillAnimation;

        [Space]

        [SerializeField] private GameObject _bunchPopup;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _rewardEffect;
        [SerializeField] private TMP_Text _popupLabel;
        [SerializeField] private GameObject _popup;
        [SerializeField] private GameObject _rawImage;

        public bool IsRewarded { get; private set; }

        public bool IsClaimed { get; private set; }

        private void Start()
        {
            _stateMachine.On<SceneLoading>(ApplyRewardIfNeeded);
        }

        private void Update()
        {
            if (!_isButton)
            {
                if (DInput.IsMousePressed)
                    Claim();
            }
        }

        protected override async void OnClick()
        {
            if (_isButton)
                Claim();
        }

        private async void Claim()
        {
            if (IsClaimed)
                return;

            IsClaimed = true;

            if (_isBunchEffect)
            {
                if (_bunchPopup != null)
                {
                    _bunchPopup.On();
                    _bunchPopup.ChildrenGet<TMP_Text>().IfNotNull(t => t.text = $"+{_level.Reward}");
                }
                
                _body.GetComponentsInChildren<MaskableGraphic>()
                    .ForEach(g =>
                    {
                        g.DOKill();
                        g.DOFade(0, 0);
                    });

                _flyingSpawner.SpawnBunch(_body.transform.position, async () =>
                {
                    ApplyRewardIfNeeded();

                    await .5f.Seconds();

                    LoadNextLevel();
                });

                return;
            }

            _rewardEffect.On();

            if (_showBillAnimation)
                _rawImage.On();

            _popup.On();
            _popupLabel.text = "+" + _level.Reward;

            if (_showClaimAnimation)
            {
                _body.GetComponentsInChildren<MaskableGraphic>()
                    .ForEach(g => g.DOFade(0, 0f));

                await .3f.Seconds();

                ApplyRewardIfNeeded();

                await 2f.Seconds();
            }

            LoadNextLevel();
        }

        private static void LoadNextLevel()
        {
            _sceneLoader.ReloadCurrentScene();
            
            // _sceneLoader.LoadLevel(_level.GetNextSceneNumber());
        }

        private void ApplyRewardIfNeeded()
        {
            if (IsRewarded) 
                return;
            
            IsRewarded = true;
            _db.Money.Value += _level.Reward;
        }
    }
}