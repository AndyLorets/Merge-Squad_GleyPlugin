using System;
using D2D.Databases;
using D2D.UI;
using D2D.Utilities;
using D2D.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.UI
{
    public class PurchaseButton : DButton
    {
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI[] otherActivityAssociatedLabels;
        
        public float Cost
        {
            get => _cost;
            set
            {
                priceLabel.text = value.Round() + "$";
                _cost = value;
            }
        }

        private float _cost;

        private void OnEnable()
        {
            _db.Money.Changed += UpdateInteractivity;

            UpdateInteractivity(_db.Money.Value);
        }

        private void OnDisable()
        {
            _db.Money.Changed -= UpdateInteractivity;
        }

        private void UpdateInteractivity(float money)
        {
            IsInteractive = money >= _cost;
            
            foreach (var l in otherActivityAssociatedLabels)
            {
                l.DOFade(IsInteractive ? 1 : _disabledAlpha, 0);
            }
        }
    }
}