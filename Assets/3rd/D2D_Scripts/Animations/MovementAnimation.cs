using System;
using AV.Inspector.Runtime;
using D2D.Common;
using D2D.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace D2D.Animations
{
    public class MovementAnimation : DAnimation
    {
        [SerializeField] protected Axis _axis;
        
        [SerializeField] private Vector3 _endPoint;
        [SerializeField] private Vector3 _endPoint2;
        [SerializeField] private bool _isHalf;
        [SerializeField] private bool _recalculateEveryLoop;
        // [SerializeField] private bool _isHookMode;

        [HideInInspector] public bool isEndPointModee = true;

        private Vector3 CalculatedDestination
        {
            get
            {
                if (_calculatedDestination == null)
                {
                    if (isEndPointModee)
                    {
                        _calculatedDestination = _endPoint;

                        if (isRandomnessSupported)
                            _calculatedDestination = DMath.RandomPointInsideBox(_endPoint, _endPoint2);
                    }
                    else
                    {
                        _calculatedDestination = new Vector3(CalculatedTo, 0);

                        if (_axis == Axis.Y)
                            _calculatedDestination = new Vector3(0, CalculatedTo);
                        else if (_axis == Axis.Z)
                            _calculatedDestination = new Vector3(0, 0, CalculatedTo);
                    }
                }

                return _calculatedDestination.Value;
            }
            set => _calculatedDestination = value;
        }
        private Vector3? _calculatedDestination = null;
        private bool _isCustomDestination;

        protected override void OnLoop()
        {
            /*if (_recalculateEveryLoop)
            {
                _calculatedDestination = null;
                var p = CalculatedDestination;
                ((Tweener)CurrentTween).ChangeEndValue(p);
            }*/
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            _calculatedDestination = null;
            var p = CalculatedDestination;
            if (_isRelative)
                p += transform.position;
            Gizmos.DrawLine(transform.position, p);
            Gizmos.DrawSphere(p, .1f);
        }

        public override DAnimation SetEndPoint(Vector3 endPoint)
        {
            CalculatedDestination = endPoint;
            isEndPointModee = true;
            _isCustomDestination = true;
            
            // ReInit();
            
            return this;
        }

        /*protected override void OnReset()
        {
            if (!isEndPointMode)
                _calculatedDestination = null;
        }*/

        protected override void OnBeforeFirstPlayAndInit()
        {
            if (_isHalf)
            {
                if (_isLocal)
                    Target.localPosition -= CalculatedDestination;
                else
                    Target.position -= CalculatedDestination;
                
                CalculatedDestination *= 2;
                CalculatedDuration *= 2;
            }
        }

        protected override Tween CreateTween()
        {
            if (!_isCustomDestination)
                _calculatedDestination = null;
            
            Tween tween = _isLocal ? 
                Target.DOLocalMove(CalculatedDestination, CalculatedDuration) : 
                Target.DOMove(CalculatedDestination, CalculatedDuration);

            /*if (_isFrom)
            {
                tween = Target.DOMove(Target.position, CalculatedDuration).From(_destination);
            }
            else
            {
                tween = Target.DOMove(_destination, CalculatedDuration);
            }*/

            return tween;
        }
        
    }
}