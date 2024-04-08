using DG.Tweening;
using UnityEngine;

namespace D2D.UI
{
    public class SizeUIAnim : UIAnim
    {
        [SerializeField] protected float from = 1;
        [SerializeField] private float to = .9f;
        [SerializeField] private bool _isRelative;
        
        protected override void Positive()
        {
            transform.DOScale(to, duration).SetRelative(_isRelative);
        }

        protected override void Negative()
        {
            transform.DOScale(from, duration).SetRelative(_isRelative);
        }
    }
}