using TMPro;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class PowerText : MonoBehaviour
{
    private TextMeshProUGUI textPro;

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();

        //UpdatePowerText(0);

        //_gameProgress.OnLevelUp += UpdatePowerText;
    }
    private void UpdatePowerText(int level)
    {
        textPro.text = $"LVL {level}";
    }
}