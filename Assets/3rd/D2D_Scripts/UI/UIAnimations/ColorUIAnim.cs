using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace D2D.UI
{
    public class ColorUIAnim : UIAnim
    {
        [SerializeField] private Color to = new Color(0, 0, 0, .7f);
        [SerializeField] private bool isAlphaOnly;
        [SerializeField] private bool isRelative;
        [SerializeField] private List<MaskableGraphic> targetGraphics;

        private List<Color> originalColors = new List<Color>();

        private void OnDrawGizmosSelected()
        {
            if (targetGraphics == null || targetGraphics.Count == 0)
                targetGraphics = GetComponentsInChildren<MaskableGraphic>().ToList();
        }
        
        private void Awake()
        {
            foreach (var g in targetGraphics)
                originalColors.Add(g.color);
        }
        
        protected override void Positive()
        {
            int i = 0;
            foreach (var g in targetGraphics)
            {
                if (isAlphaOnly)
                {
                    g.DOFade(isRelative ? originalColors[i].a + to.a : to.a, duration);
                }
                else
                {
                    g.DOColor(isRelative ? originalColors[i] + to : to, duration);
                }
                
                i++;
            }
        }

        protected override void Negative()
        {
            int i = 0;
            foreach (var g in targetGraphics)
            {
                if (isAlphaOnly)
                {
                    g.DOFade(originalColors[i].a, duration);
                }
                else
                {
                    g.DOColor(originalColors[i], duration);
                }

                i++;
            }
        }
    }
}