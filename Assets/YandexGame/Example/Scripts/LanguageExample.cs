using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LanguageExample : MonoBehaviour
{
    [SerializeField] string ru;
    [SerializeField] string en;

    TextMeshProUGUI textPro; 
    Text text;

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();
        text = GetComponent<Text>();
  
        SwitchLanguage(YandexGame.savesData.language);
    }
    private string TextObj
    {
        get
        {
            if(textPro != null)
                return textPro.text;
            if (text != null)
                return text.text;

            return null; 
        }
        set
        {
            if (textPro != null)
                textPro.text = value;
            if (text != null)
                text.text = value;
        }
    }
    private void OnEnable() => YandexGame.SwitchLangEvent += SwitchLanguage;
    private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;

    public void SwitchLanguage(string lang)
    {
        switch (lang)
        {
            case "ru":
                TextObj = ru;
                break;
            default:
                TextObj = en;
                break;
        }
    }
    public static string GetCurrentLanguage(string en, string ru)
    {
        switch (YandexGame.savesData.language)
        {
            case "ru":
                return ru;
            default:
                return en;
        }
    }
}