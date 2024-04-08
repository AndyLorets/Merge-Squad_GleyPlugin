using System;
using System.Linq;
using DG.Tweening;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace D2D.UI
{
    /// <summary>
    /// Lighter button class with more events.
    /// </summary>
    public class DButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UltEvent Clicked;

        [Tooltip("Alpha on button disabled")]
        [SerializeField] protected float _disabledAlpha = .7f;
        
        [Tooltip("For immediate clicks")]
        [SerializeField] private bool _clickIsPointerDown;
        
        [HideInInspector] public UltEvent PointerDown;
        [HideInInspector] public UltEvent PointerUp;
        [HideInInspector] public UltEvent MouseEnter;
        [HideInInspector] public UltEvent MouseExit;

        [HideInInspector] public bool needShowAdvancedProperties;

        public bool IsInteractive
        {
            get => _isInteractive;
            set
            {
                if (_isInteractive != value)
                {
                    GetComponentsInChildren<MaskableGraphic>()?.ToList().
                        ForEach(m => m.DOFade(value ? 1 : _disabledAlpha, 0));
                }
                
                _isInteractive = value;
            }
        }

        private bool _isInteractive = true;
        
        [ContextMenu("Switch advanced")]
        private void SetModeToNumeric() => needShowAdvancedProperties = !needShowAdvancedProperties;

        public void SetPointerDownAsClick() => _clickIsPointerDown = true;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isInteractive)
                return;
            
            if (!_clickIsPointerDown)
                Clicked?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isInteractive)
                return;
            
            PointerDown?.Invoke();
            
            if (_clickIsPointerDown)
                Clicked?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isInteractive)
                return;
            
            PointerUp?.Invoke();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isInteractive)
                return;
            
            MouseEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isInteractive)
                return;
            
            MouseExit?.Invoke();
        }
    }
}