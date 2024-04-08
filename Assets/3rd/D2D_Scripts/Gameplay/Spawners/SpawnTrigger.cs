using System;
using UnityEngine;

namespace D2D.Gameplay
{
    [RequireComponent(typeof(ISpawner))]
    public class SpawnTrigger : MonoBehaviour
    {
        private ISpawner Spawner
        {
            get
            {
                if (_spawner == null)
                    _spawner = GetComponent<ISpawner>();

                return _spawner;
            }
        }

        private ISpawner _spawner;

        protected GameObject Trigger()
        {
            return Spawner.Spawn();
        }
    }
}