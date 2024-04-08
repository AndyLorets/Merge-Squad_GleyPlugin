using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using AV.Inspector.Runtime;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;
using UnityEditor;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class DRay : SmartScriptAlloc
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float _distance;
        [SerializeField] private float _checkFramesDelay = -1;
        [ReadOnly] [SerializeField] private bool _is;

        [SerializeField] private RaycastHit? _hit;

        public RaycastHit? Hit => _hit;

        public bool IsHit => _is;

        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _is ? Color.green : Color.red;

            var p1 = transform.position;
            var p2 = p1 + transform.forward * _distance;
            
            Handles.DrawBezier(p1,p2,p1,p2, Gizmos.color, null, 10f);
            
            Gizmos.DrawSphere(p2, .05f);
        }
        
        #endif

        private void FixedUpdate()
        {
            if (_checkFramesDelay < 0)
            {
                CheckRaycast();
            }
            else
            {
                if (Time.frameCount % _checkFramesDelay == 0)
                    CheckRaycast();
            }
        }

        private void CheckRaycast()
        {
            _hit = transform.GetForwardHit(_distance, _layer);
            _is = _hit.HasValue;
        }
    }
}