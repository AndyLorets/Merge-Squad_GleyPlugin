using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Utilities
{
    [Serializable]
    public class RangeCurve
    {
        [SerializeField] private float factor;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Vector2 range;
        
        public float Min => Mathf.Min(range.x, range.y);
        public float Max => Mathf.Max(range.x, range.y);

        public float Compute(float value)
        {
            var dist = Max - Min;

            if (value < Min)
                return curve.Evaluate(0) * factor;
            
            if (value > Max)
                return curve.Evaluate(1) * factor;

            return curve.Evaluate((value - Min) / dist * factor);
        }
    }
}