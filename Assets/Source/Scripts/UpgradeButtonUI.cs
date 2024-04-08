using System;
using D2D.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private Button upgradeButton;

    public Button UpgradeButton => upgradeButton;

    private void OnEnable()
    {
        UpgradeButton.onClick.AddListener(() => transform.DOPunchScale(Vector3.one * .2f, .4f, 1, 1).SetRelative());
    }

    public void InitButtonUI(Sprite icon, string text)
    {
        upgradeIcon.sprite = icon;
        upgradeText.text = text;
    }
}