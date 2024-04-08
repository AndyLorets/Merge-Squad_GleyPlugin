using D2D.Utilities;
using UnityEngine;

namespace D2D.UI
{
    [RequireComponent(typeof(DButton))]
    public abstract class DButtonListener : SmartScript
    {
        protected DButton Button { get; private set; }

        protected virtual void OnEnable()
        {
            Button = GetComponent<DButton>();
            Button.Clicked += OnClick;
        }

        protected virtual void OnDisable()
        {
            Button.Clicked -= OnClick;
        }

        protected abstract void OnClick();
    }
}