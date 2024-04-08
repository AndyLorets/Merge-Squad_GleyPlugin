using D2D.Common;
using D2D.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace D2D.Animations
{
    public class RotateAnimation : DAnimation
    {
        [SerializeField] private Axis _axis = Axis.Z;
        [SerializeField] private Vector3 _endPoint;
        [SerializeField] private Vector3 _endPoint2;
        // [SerializeField] private bool _isLocal;
        
        [HideInInspector] public bool isEndPointMode = true;
        
        private Vector3 CalculatedDestination
        {
            get
            {
                if (_calculatedDestination == null)
                {
                    _calculatedDestination = new Vector3(_axis == Axis.X ? 1 : 0,
                        _axis == Axis.Y ? 1 : 0, _axis == Axis.Z ? 1 : 0) * CalculatedTo;

                    if (isEndPointMode)
                        _calculatedDestination = _endPoint;
                    
                    if (isRandomnessSupported)
                        _calculatedDestination = DMath.RandomPointInsideBox(_endPoint, _endPoint2);
                }

                return _calculatedDestination.Value;
            }
            set => _calculatedDestination = value;
        }
        private Vector3? _calculatedDestination = null;
        private bool _isCustomDestination;
        
        public override DAnimation SetEndPoint(Vector3 endPoint)
        {
            CalculatedDestination = endPoint;
            isEndPointMode = true;
            _isCustomDestination = true;
            
            // ReInit();
            
            return this;
        }

        protected override Tween CreateTween()
        {
            if (!_isCustomDestination)
                _calculatedDestination = null;

            return _isLocal ? Target.DOLocalRotate(CalculatedDestination, CalculatedDuration, RotateMode.FastBeyond360) :
                Target.DORotate(CalculatedDestination, CalculatedDuration, RotateMode.FastBeyond360);
        }
    }
}