using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class ExplosionForceApplier : SmartScript
    {
        [SerializeField] private Vector2 _force;
        [SerializeField] private Vector2 _radius;
        [SerializeField] private float _upwardsModifier;
        [SerializeField] private Transform _origin;
        [SerializeField] private bool _showGizmoo = true;

        private void OnDrawGizmosSelected()
        {
            if (!_showGizmoo)
                return;
            
            Gizmos.color = Color.red;
            
            if (_origin == null)
                _origin = transform;
            
            Gizmos.DrawSphere(_origin.position, (_radius.x + _radius.y)/2f);
        }

        private void OnEnable()
        {
            if (_origin == null)
                _origin = transform;
            
            ChildrenGets<Rigidbody>()
                .ForEach(g => g.AddExplosionForce(_force.RandomFloat(), _origin.position, _radius.RandomFloat(), _upwardsModifier));
        }
    }
}