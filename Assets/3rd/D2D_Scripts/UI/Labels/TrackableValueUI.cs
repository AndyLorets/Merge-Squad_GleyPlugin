using System;
using D2D.Common;
using D2D.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum PunchMode
{
    Icon,
    Label,
    Both,
    None,
}

namespace D2D.UI
{
    public abstract class TrackableValueUI<T> : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Transform _icon;

        [Space]
        
        [SerializeField] private PunchMode _punchMode = PunchMode.Icon;
        [SerializeField] private float _numericAnimationDuration = -1;
        [SerializeField] private float _numericAnimationDelay = 0;

        private bool CanPunch => Time.time - _timeSinceStart > .1f && _punchMode != PunchMode.None;
        
        private TrackableValue<T> _trackable;

        private Vector3 _iconOriginalLocalScale;
        private Vector3 _labelOriginalLocalScale;

        private float _timeSinceStart;
        private float _lastDesired;
        private Tween _punchTween;

        private void OnEnable()
        {
            _trackable = FindTrackable();
            
            if (_icon != null)
                _iconOriginalLocalScale = _icon.localScale;

            if (_label != null)
                _labelOriginalLocalScale = _label.transform.localScale;
            
            _timeSinceStart = Time.time;

            _trackable.Changed += Redraw;
            
            Redraw(_trackable.Value);
        }

        private void OnDisable()
        {
            _trackable.Changed -= Redraw;
        }

        private void Redraw(T newValue)
        {
            if (_numericAnimationDuration > 0)
            {
                PlayNumericAnimation(newValue);
            }
            else
            {
                _label.text = ValueToText(newValue);
                
                if (CanPunch)
                    PlayPunch();
            }
        }

        private void PlayNumericAnimation(T newValue)
        {
            float desired = newValue switch
            {
                double d => (float) d,
                float f => f,
                int i => i,
                _ => throw new Exception("If you want numeric animation, label also should be float or int!")
            };
            
            float current = _lastDesired;
            _lastDesired = desired;

            var tween = DOTween.To(
                () => current,
                x =>
                {
                    current = x;
                    _label.text = FloatToText(current);
                },
                desired,
                _numericAnimationDuration
            );
            
            tween.SetDelay(_numericAnimationDelay.LimitMin(0));
            tween.onComplete += PlayPunch;
        }

        private void PlayPunch()
        {
            Transform punchTarget = null;
            bool canPunchIcon = _punchMode == PunchMode.Icon || _punchMode == PunchMode.Both;
            bool canPunchLabel = _punchMode == PunchMode.Label || _punchMode == PunchMode.Both;
            
            if (canPunchIcon && _icon != null)
            {
                _icon.transform.localScale = _iconOriginalLocalScale;
                punchTarget = _icon;
            }

            if (canPunchLabel && _label != null)
            {
                _label.transform.localScale = _labelOriginalLocalScale;
                punchTarget = _label.transform;
            }
            
            _punchTween?.Kill();
            _punchTween = punchTarget.PunchUI();
        }
        
        /// <summary>
        /// Called only once.
        /// </summary>
        public abstract TrackableValue<T> FindTrackable();

        /// <summary>
        /// Custom convert value to text.
        /// </summary>
        protected virtual string ValueToText(T value)
        {
            return value.ToString();
        }
        
        /// <summary>
        /// Custom convert float to text (for numerics).
        /// </summary>
        protected virtual string FloatToText(float value)
        {
            return value.Round().ToString();
        }
    }
}