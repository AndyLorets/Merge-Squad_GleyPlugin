using System;
using System.Linq;
using _3rd.D2D_Scripts.Animations;
using D2D.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace D2D.Animations
{
    /// <summary>
    /// TODO: Add mesh fade!
    /// </summary>
    public class FadeAnimation : DAnimation
    {
        protected override Tween CreateTween()
        {
            Tween tween = null;
            
            var spriteRenderer = Target.Get<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                tween = spriteRenderer.DOFade(CalculatedTo, CalculatedDuration);
            }
            
            var maskable = Target.ChildrenGets<MaskableGraphic>().Where(m => m.gameObject == gameObject || m.gameObject != gameObject && !m.Is<AvoidChildrenFade>()).ToArray();
            if (!maskable.IsNullOrEmpty())
            {
                tween = _isFrom ? maskable[0].DOFade(maskable[0].color.a, CalculatedDuration).From(CalculatedTo) :
                    maskable[0].DOFade(CalculatedTo, CalculatedDuration);

                if (maskable.Length > 1)
                {
                    maskable.Skip(1).ForEach(m =>
                    {
                        if (_isFrom)
                            m.DOFade(m.color.a, CalculatedDuration).From(CalculatedTo);
                        else
                            m.DOFade(CalculatedTo, CalculatedDuration);
                    });
                }
            }

            if (tween == null)
                throw new Exception("Can`t fade because there is no maskable or SpriteRenderer!");

            return tween;
        }
    }
}