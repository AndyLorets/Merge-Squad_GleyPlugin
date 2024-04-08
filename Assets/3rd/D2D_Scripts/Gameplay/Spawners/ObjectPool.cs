using System;
using System.Collections.Generic;
using UnityEngine;

namespace D2D.Gameplay
{
    /// <summary>
    /// Use it if there is lots of objects and they don`t contain any logic.
    /// Saves memory and performance.
    /// </summary>
    public class ObjectPool : MonoBehaviour, ISpawner
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _size;
        
        private Queue<GameObject> _reservedObjects;

        public void Init(GameObject prefab, int size)
        {
            _prefab = prefab;
            _size = size;
            
            if (_reservedObjects != null)
            {
                foreach (GameObject o in _reservedObjects)
                {
                    Destroy(o);
                }

                _reservedObjects.Clear();
            }

            _reservedObjects = new Queue<GameObject>();

            // Create lots of disabled objects which will be used later
            for (int i = 0; i < size; i++)
            {
                var instance = Instantiate(prefab, transform, true);
                instance.SetActive(false);
                _reservedObjects.Enqueue(instance);
            }
        }

        public GameObject Spawn()
        {
            if (_prefab == null || _reservedObjects == null)
            {
                throw new Exception("Object pool isn't initialized!");
            }

            // Pull the oldest object, use it and push it to beginning
            var instance = _reservedObjects.Dequeue();
            instance.SetActive(true);

            // To avoid "speed up" effect
            var rb = instance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            _reservedObjects.Enqueue(instance);

            return instance;
        }
    }
}
