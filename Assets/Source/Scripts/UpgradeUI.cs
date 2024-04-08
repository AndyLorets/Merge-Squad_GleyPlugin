using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UpgradeButtonUI[] buttons;

    private void Awake()
    {
        HideUI();
    }

    public UpgradeButtonUI[] GetButtons()
    {
        return buttons;
    }

    public void ShowUI()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        Time.timeScale = 0;
    }
    public void HideUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        Time.timeScale = 1;
    }
}