using System.Collections.Generic;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D
{
    public class PrefabSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private List<GameObject> _prefabs;
        
        public GameObject Spawn()
        {
            return Instantiate(_prefabs.GetRandomElement(), 
                transform.position, Quaternion.identity, transform);
        }
    }
}