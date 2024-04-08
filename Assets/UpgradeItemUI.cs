using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image lockIcon;

    public void Init(Sprite icon, bool isUnlocked)
    {
        itemIcon.sprite = icon;
        lockIcon.gameObject.SetActive(isUnlocked);
    }
    public void Unlock()
    {
        lockIcon.gameObject.SetActive(false);
    }
}