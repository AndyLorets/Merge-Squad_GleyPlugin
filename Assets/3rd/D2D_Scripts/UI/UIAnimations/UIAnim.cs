using System;
using UnityEngine;

namespace D2D.UI
{
    public enum UIEvent {Click, Hover, /* Window */ }
    
    /// <summary>
    /// Plays animations when attached event invokes.
    /// This class is not so flexible, but this done to save time managing and connecting
    /// variables in inspector, thus there is not so many ways to expand this class.
    /// </summary>
    public abstract class UIAnim : MonoBehaviour
    {
        [SerializeField] private UIEvent eventType;
        [SerializeField] protected float duration = .2f;

        private bool IsButtonRequired => eventType == UIEvent.Click || eventType == UIEvent.Hover;

        // rivate bool IsWindowRequired => eventType == UIEvent.Window;

        private void OnDrawGizmos()
        {
            var button = GetComponent<DButton>();
            // var window = GetComponent<Window>();

            if (IsButtonRequired && button == null)
                gameObject.AddComponent<DButton>();

            // if (IsWindowRequired && window == null)
            //     gameObject.AddComponent<Window>();
        }

        private void OnEnable()
        {
            var button = GetComponent<DButton>();
            // var window = GetComponent<Window>();

            if (eventType == UIEvent.Click)
            {
                button.PointerDown += Positive;
                button.PointerUp += Negative;
            }

            if (eventType == UIEvent.Hover)
            {
                button.MouseEnter += Positive;
                button.MouseExit += Negative;
            }

            // if (eventType == UIEvent.Window)
            // {
            //     window.Opened += Positive;
            //     window.Closed += Negative;
            // }
        }

        protected abstract void Positive();
        protected abstract void Negative();
    }
}