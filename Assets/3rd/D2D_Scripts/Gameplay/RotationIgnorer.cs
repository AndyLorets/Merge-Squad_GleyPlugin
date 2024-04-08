using System;
using UnityEngine;

namespace D2D
{
    public class RotationIgnorer : MonoBehaviour
    {
        private Transform _target;
        
        private Vector3 _originalSwift;

        private void Start()
        {
            _originalSwift = transform.localPosition;
            
            _target = transform.parent;
            transform.parent = null;
        }

        private void Update()
        {
            transform.position = _originalSwift + _target.position;
        }
    }
}