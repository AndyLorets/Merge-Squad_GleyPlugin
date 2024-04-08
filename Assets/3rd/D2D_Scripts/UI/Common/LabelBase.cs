using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using TMPro;

namespace D2D.UI
{
    public abstract class LabelBase : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private string _preText;

        protected virtual float UpdateRate => -1;
        
        protected virtual float StartRate => 0;

        private void Start()
        {
            Redraw();
            
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