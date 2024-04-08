using System;
using D2D.Utilities;
using DG.Tweening;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Gameplay
{
    public class Health : MonoBehaviour
    {
        [Header("Hit Flash")]
        [SerializeField] private Gradient gradient;
        private Color originalColor;
        
        [SerializeField] private float _maxPoints;
        [SerializeField] private HealthData _healthData;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private GameObject _deathEffect;
        
        [SerializeField] private bool _isGrayFadeout;

        public event Action Died;
        public event Action<float> Damaged;
        public event Action PointsChanged;

        private Tween flashTween;
        
        public GameObject LastAttacker { get; private set; }

        public float CurrentPoints
        {
            get => _currentPoints;
            private set
            {
                _currentPoints = value;
                PointsChanged?.Invoke();
            }
        }

        public float MaxPoints
        {
            private set => _maxPoints = value;
            get
            {
                if (!isHealthDataMode) 
                    return _maxPoints;
                
                // If health data:
                
                if (_healthData == null)
                    throw new Exception("Health data is not attached!");

                return _healthData.maxPoints;
            }
        }

        public bool isHealthDataMode;
        public bool particlesAreEnabled = true;

        private float _currentPoints;

        private void OnDrawGizmos()
        {
            if (_maxPoints <= 0)
                _maxPoints = 1;
        }

        private void Awake()
        {
            CurrentPoints = MaxPoints;

            if (_meshRenderer != null)
            {
                originalColor = _meshRenderer.material.color;
            }
        }

        public void ApplyDamage(GameObject attacker, float damagePoints)
        {
            // Validate inputs
            if (damagePoints <= 0)
            {
                throw new Exception("Damage points should be positive!");
            }

            flashTween.KillTo0();

            // Object is already died => return
            if (CurrentPoints <= 0)
                return;

            CurrentPoints -= damagePoints;
            LastAttacker = attacker;


            if (CurrentPoints > 0)
            {
                Spawn(_hitEffect);

                if (_meshRenderer != null)
                {
                    flashTween = _meshRenderer.material.DOColor(gradient.Evaluate(_currentPoints / _maxPoints), .1f);

                    _meshRenderer.material.DOColor(originalColor, .05f).SetDelay(.1f);
                }

                Damaged?.Invoke(damagePoints);
            }
            else
            {
                Died?.Invoke();

                if (_isGrayFadeout)
                {
                    GrayFadeoutDeath();
                }
                else
                {
                    ImmediateDeath();
                }
            }
        }
        
        private void ImmediateDeath()
        {
            Spawn(_deathEffect);
            Destroy(gameObject);
        }
        
        private void GrayFadeoutDeath()
        {
            this.ChildrenGets<MeshRenderer>(renderer =>
            {
                renderer.materials.ForEach(material =>
                {
                    material.DOColor(_gameData.grayDeathColor, _gameData.grayDeathDuration);
                });
            });

            this.ChildrenGets<SkinnedMeshRenderer>(renderer =>
            {
                renderer.materials.ForEach(material =>
                {
                    material.DOColor(_gameData.grayDeathColor, _gameData.grayDeathDuration);
                });
            });
            
            _gameData.grayCorpseLifetime.AfterCall(() => transform.DOScale(0, .3f).onComplete += ImmediateDeath);

            /*var rb = this.Get<Rigidbody>(); 
            rb.isKinematic = false;
            rb.freezeRotation = false;*/
            
            /*.1f.AfterCall(() => rb.angularVelocity *= DMath.Random(1, 2f));*/
        }

        private void Spawn(GameObject prefab)
        {
            if (prefab == null)
                return;
            
            GameObject instance = Instantiate(prefab, transform.position, 
                transform.rotation, null);

            Destroy(instance, 2f);
        }

        public void Heal(float healPoints)
        {
            if (healPoints <= 0)
                throw new Exception("Heal points should be positive!");

            CurrentPoints = Math.Min(CurrentPoints+healPoints, _maxPoints);
        }

        public void SetMaxPoints(float newMaxPoints, bool needRefill = false)
        {
            if (newMaxPoints <= 0)
                throw new Exception("Max points should be positive!");

            MaxPoints = newMaxPoints;
            
            if (needRefill)
                CurrentPoints = MaxPoints;
        }
    }
}