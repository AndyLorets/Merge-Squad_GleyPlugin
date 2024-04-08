using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace D2D.Utilities
{
    public class FPSBooster : MonoBehaviour
    {
        [SerializeField] private int _desiredFPS = 60;

        [Header("For mobile")] 
        [SerializeField] private float _physFPS = 33;
        [SerializeField] private bool _shadows;
        [SerializeField] private bool _pp;

        [Button("Apply")]
        private void Apply()
        {
            Time.fixedDeltaTime = 1f / _physFPS;
            this.Finds<Light>().ForEach(l => l.shadows = _shadows ? LightShadows.Hard : LightShadows.None);
            Resources.FindObjectsOfTypeAll<PostProcessLayer>().ForEach(p => p.enabled = _pp);
        }

        private void Start()
        {
            Application.targetFrameRate = _desiredFPS;
        
            // #if UNITY_EDITOR
            //     Debug.unityLogger.logEnabled = true;
            // #else
            //     Debug.unityLogger.logEnabled = false;
            // #endif
        }
    }
}