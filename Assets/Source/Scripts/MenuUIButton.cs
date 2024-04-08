using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI levelText;

    public Button Button => button;
    public TextMeshProUGUI PriceText => priceText;
    public TextMeshProUGUI LevelText => levelText;
    
    private void OnEnable()
    {
        Button.onClick.AddListener(() => transform.DOPunchScale(Vector3.one * .2f, .4f, 1, 1).SetRelative());
    }
}