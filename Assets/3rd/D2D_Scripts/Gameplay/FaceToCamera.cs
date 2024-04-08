using System;
using UnityEngine;

namespace D2D
{
    public class FaceToCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            transform.LookAt(_mainCamera.transform);
        }
    }
}