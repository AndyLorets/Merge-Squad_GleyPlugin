using D2D.Core;
using D2D.Gameplay;
using UnityEngine;

namespace D2D
{
    public class SpeedWindEffect : GameStateMachineUser
    {
        [SerializeField] private float _defaultRateOverTime = 10;

        // private PlayerMovement _playerMovement;
        private ParticleSystem _particleSystem;

        private void Start()
        {
            // _playerMovement = FindObjectOfType<PlayerMovement>();
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            var emissionModule = _particleSystem.emission;
            // emissionModule.rateOverTime = _defaultRateOverTime * _playerMovement.speedFactor;
        }

        protected override void OnGameFinish()
        {
            Destroy(gameObject);
        }
    }
}