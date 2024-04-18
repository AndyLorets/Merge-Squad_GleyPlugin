using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LanguageExample : MonoBehaviour
{
    [SerializeField] string ru;
    [SerializeField] string en;
    [Space]
    [SerializeField] Sprite imgRu;
    [SerializeField] Sprite imgEn;

    TextMeshProUGUI textPro; 
    Text text;
    Image _image; 

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();
        text = GetComponent<Text>();
        _image = GetComponent<Image>(); 


        SwitchLanguage(YandexGame.savesData.language);
    }
    private Sprite ImageObj
    {
        get
        {
            if (_image != null)
                return _image.sprite;

            return null;
        }
        set
        {
            if (_image != null)
                _image.sprite = value;
        }
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
                ImageObj = imgRu; 
                break;
            default:
                TextObj = en;
                ImageObj = imgEn;
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