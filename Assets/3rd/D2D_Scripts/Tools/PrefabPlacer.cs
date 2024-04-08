using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Gameplay;

namespace D2D
{
    public class PrefabPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private Vector3 _offset;

        public GameObject[] Prefabs => _prefabs;

        public Vector3 Offset => _offset;
    }
}