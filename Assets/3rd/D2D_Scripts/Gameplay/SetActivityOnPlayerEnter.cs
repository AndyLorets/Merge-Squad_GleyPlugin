using System;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D
{
    public class SetActivityOnPlayerEnter : MonoBehaviour
    {
        [SerializeField] private bool _activityStateOnPlayerEnter;
        [SerializeField] private GameObject _target;

        private void OnTriggerEnter(Collider other)
        {
            if (other.Is<Player>())
                _target.SetActive(_activityStateOnPlayerEnter);
        }
    }
}