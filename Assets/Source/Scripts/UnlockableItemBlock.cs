using D2D;
using D2D.Core;
using D2D.Utilities;
using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class UnlockableItemBlock : GameStateMachineUser
{
    [SerializeField] private Image unlockBack;
    [SerializeField] private Image unlockFill;
    [SerializeField] private Image unlockOutline;
    [SerializeField] private Image godrays;
    [SerializeField] private TextMeshProUGUI unlockText;
    [SerializeField] private CanvasGroup finishButton;

    [Header("Fill Settings")]
    [SerializeField] private float fillTime = 2f;

    private UnlockableItem unlockableItem;

    private bool isUnlocked = false;

    private void Start()
    {
        if (_db.UnlockableItem.Value == "")
        {
            _db.UnlockableItem.Value = _gameData.firstUnlockable.name;

            unlockableItem = _gameData.firstUnlockable;
        }
        else
        {
            unlockableItem = _gameData.unlockables.First(x => x.name == _db.UnlockableItem.Value);
        }

        if (unlockableItem.UnlockableType == UnlockableType.Member)
        {
            var member = (UnlockableMember)unlockableItem;
            
            if (_db.UnlockedMembers.Contains(member.MemberUpgradesSO.name))
            {
                unlockableItem = null;
            }
        }

        if (unlockableItem == null)
        {
            FinishAnimation();

            unlockBack.gameObject.Off();
            unlockText.gameObject.Off();
            godrays.gameObject.Off();

            return;
        }

        SetUnlockable();
        StartAnimation();
    }

    private void SetUnlockable()
    {
        unlockBack.sprite = unlockableItem.Icon;
        unlockFill.sprite = unlockableItem.Icon;
        unlockOutline.sprite = unlockableItem.BackIcon;
    }
    public void StartAnimation()
    {
        var startProgress = _db.UnlockableItemProgress.Value;

        unlockFill.fillAmount = startProgress;
        unlockFill.DOFillAmount(startProgress + unlockableItem.UnlockStep, fillTime).OnUpdate(UpdateFillText).OnComplete(FinishAnimation);

        if (startProgress + unlockableItem.UnlockStep >= 1f)
        {
            UnlockItem();
        }
        else
        {
            _db.UnlockableItemProgress.Value = startProgress + unlockableItem.UnlockStep;
        }
    }
    private void UpdateFillText()
    {
        unlockText.text = Mathf.CeilToInt(unlockFill.fillAmount * 100).ToString() + "%";
    }

    private void UnlockItem()
    {
        if (unlockableItem.UnlockableType == UnlockableType.Member)
        {
            var member = (UnlockableMember)unlockableItem;

            _db.UnlockedMembers.Add(member.MemberUpgradesSO.name);
            _db.SaveMembers();
            _db.LastUnlockedMember.Value = member.MemberUpgradesSO.name;

            if (member.NextUnlockable != null)
            {
                _db.UnlockableItem.Value = member.NextUnlockable.name;
                _db.UnlockableItemProgress.Value = 0;
            }
            else
            {
                _db.UnlockableItemProgress.Value = 1.1f;
            }
        }

        isUnlocked = true;
    }
    private void UnlockedAnimation()
    {
        unlockText.text = "YOU'VE UNLOCKED\n" + unlockableItem.ShowName;
    }
    public void FinishAnimation()
    {
        if (isUnlocked)
        {
            UnlockedAnimation();

            unlockBack.transform.DOPunchScale(Vector3.one * .5f, .6f, 1, .5f);
            unlockText.transform.DOPunchScale(Vector3.one * .5f, .6f, 1, .5f);
        }

        finishButton.DOFade(1, .5f).
            OnComplete(() => 
            {
                finishButton.interactable = true;
                finishButton.blocksRaycasts = true;
            }
            );
    }
}