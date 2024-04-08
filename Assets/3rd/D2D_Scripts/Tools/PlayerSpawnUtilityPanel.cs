using System;
using D2D.Gameplay;
using UnityEngine;

namespace D2D
{
    public class PlayerSpawnUtilityPanel : MonoBehaviour
    {
        [SerializeField] private Transform _currentSpawn;

        private void OnValidate()
        {
            if (_currentSpawn == null)
                return;
            
            FindObjectOfType<Player>().transform.position = _currentSpawn.position;
        }
    }
}