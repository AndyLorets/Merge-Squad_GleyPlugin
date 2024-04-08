using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MPUIKIT;
using UnityEngine;

namespace D2D.UI
{
    public class RoundnessUIAnim : UIAnim
    {
        [SerializeField] private float to = 12;
        [SerializeField] private List<MPImage> targets;

        private Vector4 originalRadius;

        private void OnDrawGizmosSelected()
        {
            if (targets == null || targets.Count == 0)
                targets = GetComponentsInChildren<MPImage>().ToList();
        }

        private void Awake()
        {
            originalRadius = targets[0].Rectangle.CornerRadius;
        }

        private void Animate(float endValue)
        {
            foreach (var t in targets)
            {
                DOTween.To(
                    () => t.Rectangle.CornerRadius.x,
                    x =>
                    {
                        var rect = t.Rectangle;
                        rect.CornerRadius = new Vector4(x, x, x, x);
                        t.Rectangle = rect;
                    }, 
                    endValue,
                    duration);
            }
        }
        
        protected override void Positive()
        {
            Animate(to);
        }

        protected override void Negative()
        {
            Animate(originalRadius.x);
        }
    }
}