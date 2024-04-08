using D2D.Utilities;
using D2D.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace D2D.Animations
{
    public class ScaleAnimation : DAnimation
    {
        protected override Tween CreateTween()
        {
            return _isFrom ? Target.DOScale(Target.localScale.x, CalculatedDuration).From(CalculatedTo) :
                Target.DOScale(CalculatedTo, CalculatedDuration);
        }
    }
}