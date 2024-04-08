using System;
using System.Collections;
using System.Collections.Generic;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D
{
    public class RandomChildrenSwitcher : MonoBehaviour
    {
        public GameObject ActiveGameObject { get; private set; }
        
        private void Start()
        {
            var models = new List<GameObject>();
            foreach (Transform child in transform)
            {
                models.Add(child.gameObject);
                child.gameObject.Off();
            }

            ActiveGameObject = models.GetRandomElement();
            ActiveGameObject.On();
        }
    }
}