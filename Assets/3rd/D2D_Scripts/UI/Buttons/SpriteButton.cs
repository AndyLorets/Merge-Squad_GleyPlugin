using TMPro;
using UnityEngine;

namespace D2D.UI
{
    /// <summary>
    /// Big and juicy button. The opposite of flat button
    /// </summary>
    public class SpriteButton : DButton
    {
        [SerializeField] private RectTransform foreground;
        [SerializeField] private RectTransform background;
        [SerializeField] private TextMeshProUGUI text;

        private Vector3 releasePosition;
        private Vector3 pressPosition;
    
        private RectTransform textRect;

        private void OnEnable()
        {
            releasePosition = foreground.anchoredPosition;
            pressPosition = background.anchoredPosition;

            if (text != null)
                textRect = text.GetComponent<RectTransform>();
        
            PointerDown += SetPressed;
            PointerUp += SetReleased;
        }
    
        private void OnDisable()
        {
            PointerDown -= SetPressed;
            PointerUp -= SetReleased;
        }

        private void SetPressed()
        {
            foreground.anchoredPosition = pressPosition;
            background.gameObject.SetActive(false);
            
            if (text != null)
                textRect.anchoredPosition = pressPosition;
        }
    
        private void SetReleased()
        {
            foreground.anchoredPosition = releasePosition;
            background.gameObject.SetActive(true);
            
            if (text != null)
                textRect.anchoredPosition = releasePosition;
        }
    }
}
