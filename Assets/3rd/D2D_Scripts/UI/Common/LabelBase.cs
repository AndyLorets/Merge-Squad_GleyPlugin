using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using System.Data;

namespace D2D.UI
{
    public abstract class LabelBase : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        private string _preText => LanguageExample.GetCurrentLanguage("Level ", "Уровень ");

        protected virtual float UpdateRate => -1;
        
        protected virtual float StartRate => 0;

        private void Start()
        {
            Invoke(nameof(Redraw), .5f);
            
            if (UpdateRate > 0)
                InvokeRepeating(nameof(Redraw), StartRate, UpdateRate);
        }

        private void Update()
        {
            if (UpdateRate < 0)
                Redraw();
        }

        private void Redraw()
        {
            _label.text = $"{_preText}{GetText()}";
        }

        protected abstract string GetText();
    }
}