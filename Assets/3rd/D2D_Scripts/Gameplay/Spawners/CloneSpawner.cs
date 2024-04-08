using System;
using System.Collections.Generic;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    public class CloneSpawner : MonoBehaviour, ISpawner
    {
        private List<GameObject> _prefabs = new List<GameObject>();

        private void Start()
        {
            if (transform.childCount == 0)
                throw new Exception("No children in spawner!");
            
            foreach (Transform child in transform)
            {
                _prefabs.Add(child.gameObject);
            }
        }

        public GameObject Spawn()
        {
            return Instantiate(_prefabs.GetRandomElement(), 
                transform.position, Quaternion.identity, transform);
        }
    }
}