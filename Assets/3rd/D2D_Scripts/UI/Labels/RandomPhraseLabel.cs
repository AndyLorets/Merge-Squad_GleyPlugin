using System;
using D2D.UI;
using D2D.Utilities;
using TMPro;
using UnityEngine;

namespace D2D.UI
{
    public class RandomPhraseLabel : MonoBehaviour
    {
        [SerializeField] private string[] _phrases;

        private void Start()
        {
            var uiLabel = GetComponent<TextMeshProUGUI>();
            var worldUILabel = GetComponent<TextMeshPro>();

            if (uiLabel == null && worldUILabel == null)
            {
                throw new NullReferenceException("text wanted to have default font");
            }

            if (uiLabel != null)
                uiLabel.text = _phrases.GetRandomElement();
            
            if (worldUILabel != null)
                worldUILabel.text = _phrases.GetRandomElement();
        }
    }
}