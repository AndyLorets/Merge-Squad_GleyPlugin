using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;

namespace D2D
{
    public class FactorGameObjectSwitcher : MonoBehaviour
    {
        public float factor;
        
        private Transform[] _children;

        private void Start()
        {
            _children = transform.GetChildTransforms().ToArray();
        }

        private void Update()
        {
            var l = _children.Length;
            var index = Mathf.FloorToInt(factor * l).Clamp(0, l - 1);
            for (int i = 0; i < _children.Length; i++)
            {
                if (i == index)
                    _children[i].gameObject.On();
                else
                    _children[i].gameObject.Off();
            }
        }
    }
}