using D2D;
using D2D.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class CharacterItemUI : Unit
{
    [SerializeField] private TextMeshProUGUI memberName;
    [SerializeField] private Image memberIcon;
    [SerializeField] private Image background;
    [SerializeField] private Image LockImage;
    [SerializeField] private Image LockedLayer;
    [SerializeField] private Image availableImage;

    public void Init(MemberUpgrades member)
    {
        memberName.text = member.UpgradeText;
        memberIcon.sprite = member.SilhouetteIcon;
        LockedLayer.sprite = member.SilhouetteIcon;
    }

    public void Unlock()
    {
        LockImage.DOFade(0, 0);
        LockedLayer.DOFade(0, 0);
        availableImage.gameObject.On();
    }
}