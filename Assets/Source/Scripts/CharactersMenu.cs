using D2D;
using D2D.Utilities;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class CharactersMenu : Unit
{
    [SerializeField] private MemberUpgrades[] members;
    [SerializeField] private GameObject memberUIPrefab;
    [SerializeField] private Transform memberUIRoot;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private Dictionary<MemberUpgrades, CharacterItemUI> UIbyCharacter = new Dictionary<MemberUpgrades, CharacterItemUI>();

    private void Start()
    {
        List<MemberUpgrades> membersList = new();

        foreach (var member in members)
        {
            UIbyCharacter.Add(member, null);
            membersList.Add(member);
        }

        var newList = membersList.OrderByDescending(x => _db.UnlockedMembers.Contains(x.name) ? 1 : 0);
        
        foreach (var member in newList)
        {
            var memberGO = Instantiate(memberUIPrefab, memberUIRoot);
            var memberComp = memberGO.Get<CharacterItemUI>();

            memberComp.Init(member);

            if (_db.UnlockedMembers.Contains(member.name))
            {
                memberComp.Unlock();
            }

            UIbyCharacter[member] = memberComp;
        }

        closeButton.onClick.AddListener(CloseWindow);
        openButton.onClick.AddListener(OpenWindow);
    }

    public void CloseWindow()
    {
        canvasGroup.DOFade(0, 0);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void OpenWindow()
    {
        canvasGroup.DOFade(1, 0);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}