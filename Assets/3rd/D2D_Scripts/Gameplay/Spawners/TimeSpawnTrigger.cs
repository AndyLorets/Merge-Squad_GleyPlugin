using System;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    public class TimeSpawnTrigger : SpawnTrigger
    {
        [SerializeField] private float _delayBetweenSpawns;

        private void Start()
        {
            Invoke(nameof(Trigger), _delayBetweenSpawns);
        }
    }
}