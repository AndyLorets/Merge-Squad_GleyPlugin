using System;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Common
{
    public enum Axis { X, Y, Z, }
    public enum UpdateType {Update, FixedUpdate, LateUpdate}

    /// <summary>
    /// More for real game objects (obstacles or rigidbodies).
    /// For future invert delay.
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector2 _speed;
        [SerializeField] private float speed;
        [SerializeField] private bool _randomizeSign;
        [SerializeField] private bool _isLocal;

        [Space(10)]
        [SerializeField] private Axis _axis = Axis.Z;
        [SerializeField] private UpdateType _updateType;
        [SerializeField] private bool _useRigidbody;

        [HideInInspector] public float speedFactor = 1;

        public float CalculatedSpeed { get; private set; }

        private Vector2 Speed => isRandom ? _speed : Vector2.one * speed;

        public bool isRandom;
        
        private Vector3 _torqueVector;
        private Rigidbody _rigidbody;
        
        [ContextMenu("Switch Randomness")]
        private void SwitchRandomness() => isRandom = !isRandom;

        public void RecalculateSpeed()
        {
            CalculatedSpeed = Speed.RandomFloat();
            
            if (_randomizeSign)
                CalculatedSpeed *= DMath.RandomSign();
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            RecalculateSpeed();
            InitTorqueVector();
        }
        
        private void InitTorqueVector()
        {
            _torqueVector = new Vector3();
            if (_axis == Axis.X)
                _torqueVector.x = 1;
            if (_axis == Axis.Y)
                _torqueVector.y = 1;
            if (_axis == Axis.Z)
                _torqueVector.z = 1;
        }

        private void Update()
        {
            if (_updateType == UpdateType.Update)
                Rotate();
        }


        private void FixedUpdate()
        {
            if (_updateType == UpdateType.FixedUpdate)
                Rotate();
        }

        private void LateUpdate()
        {
            if (_updateType == UpdateType.LateUpdate)
                Rotate();
        }

        private void Rotate()
        {
            Vector3 rotSwift = _torqueVector * CalculatedSpeed * speedFactor * Time.deltaTime;

            if (_useRigidbody)
            {
                _rigidbody.MoveRotation(Quaternion.Euler(rotSwift));
            }
            else
            {
                if (_isLocal)
                {
                    transform.eulerAngles += rotSwift * Time.deltaTime;
                }
                else
                {
                    transform.Rotate(rotSwift);
                }
            }
        }
    }
}