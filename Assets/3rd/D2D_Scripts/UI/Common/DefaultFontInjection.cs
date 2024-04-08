using System;
using D2D.UI;
using TMPro;
using UnityEngine;

namespace D2D.UI
{
    /// <summary>
    /// Can be extended with other classes to secondary game font, etc.
    /// Or use enum for primary, secondary text. Or bold, regular. 
    /// </summary>
    public class DefaultFontInjection : MonoBehaviour
    {
        [SerializeField] private bool _isOn = true;
        
        private void Start()
        {
            return;
            
            if (!_isOn)
                return;
            
            // var defaultGameFont = UISettings.Instance.defaultGameFont;
            var defaultGameFont = FindObjectOfType<TextMeshPro>().font;
            var uiLabel = GetComponent<TextMeshProUGUI>();
            var worldUILabel = GetComponent<TextMeshPro>();

            if (uiLabel == null && worldUILabel == null)
            {
                throw new NullReferenceException("text wanted to have default font");
            }

            if (uiLabel != null)
                uiLabel.font = defaultGameFont;
            
            if (worldUILabel != null)
                worldUILabel.font = defaultGameFont;
        }
    }
}