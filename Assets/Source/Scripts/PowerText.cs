using TMPro;
using UnityEngine;

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
        string text = LanguageExample.GetCurrentLanguage("LVL", "сп."); 
        textPro.text = $"{text} {level}";
    }
}