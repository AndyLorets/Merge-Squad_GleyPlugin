using D2D.Utilities;
using UnityEngine;

namespace D2D.Gameplay
{
    /// <summary>
    /// Use this class if you want to add to your collectables or any loot a nice magnet effect.
    /// </summary>
    public class MagnetizeToPlayer : MonoBehaviour
    {
        [SerializeField] private float magnetStartForce = .1f;
        [SerializeField] private float magnetForceIncrease = 1.01f;

        private Transform _player;
        
        private Vector3 _magnetForce;
        private float _magnetForceFactor;

        private void Start()
        {
            _player = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            _magnetForceFactor *= magnetForceIncrease;
            
            var d = _player.position - transform.position;
            _magnetForce = d.normalized * _magnetForceFactor;
            
            transform.position += _magnetForce;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.Is<Player>())
                StartMagnetize();
        }

        private void StartMagnetize()
        {
            _magnetForceFactor = magnetStartForce;
        }
    }
}