using D2D.Utilities;
using UnityEngine;

namespace D2D.Gameplay
{
    public class TimeSpawner : MonoBehaviour
    {
        [SerializeField] private Transform prefab;
        [SerializeField] private Vector2 _delayBetweenSpawns;

        private float _timeOfNextSpawn;

        private void Update()
        {
            if (Time.time > _timeOfNextSpawn && enabled)
            {
                _timeOfNextSpawn = Time.time + _delayBetweenSpawns.RandomFloat();

                Instantiate(prefab, transform.position, transform.rotation);
            }
        }
    }
}
