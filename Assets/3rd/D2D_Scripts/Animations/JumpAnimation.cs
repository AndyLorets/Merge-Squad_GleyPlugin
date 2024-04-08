using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Animations;
using D2D.Gameplay;
using D2D.Utilities;
using DG.Tweening;
using DG.Tweening.Core;

namespace D2D
{
    public class JumpAnimation : DAnimation
    {
        // [SerializeField] private bool _isLocal;
        [SerializeField] private float power = 1;
        [SerializeField] private Vector2 _power = new Vector2(1, 1);
        [SerializeField] private int steps = 1;
        [SerializeField] private Vector2 _steps = new Vector2(1, 1);
        [SerializeField] private Vector3 _endPoint;
        [SerializeField] private Vector3 _endPoint2;
        
        private Vector2 Power =>
            isRandomnessSupported ? _power : new Vector2(power, power);
        
        private Vector2 Steps =>
            isRandomnessSupported ? _steps : new Vector2(steps, steps);
        
        public override DAnimation SetEndPoint(Vector3 endPoint)
        {
            _endPoint = endPoint;
            _isCustomEndPoint = true;

            ReInit();
                
            return this;
        }
        private bool _isCustomEndPoint;

        protected override Tween CreateTween()
        {
            var to = _endPoint;
            if (_isCustomEndPoint == false && isRandomnessSupported)
            {
                to = DMath.RandomPointInsideBox(_endPoint, _endPoint2);
                Debug.Log("to");
            }

            if (_isLocal)
            {
                return Target.DOLocalJump(to, Power.RandomFloat(),
                    Steps.RandomInt(), CalculatedDuration);
            }

            return Target.DOJump(to, Power.RandomFloat(),
                Steps.RandomInt(), CalculatedDuration);
        }
    }
}