using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using NaughtyAttributes;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    /// <summary>
    /// Only for fx camera hub usage.
    /// </summary>
    public class FXCamera : SmartScript
    {
        private FXRenderTexture[] _textures;
        private FXRenderTextureParticle[] _particles;

        private Dictionary<FXRenderTextureType, float> _lastTimeUsages = 
            new Dictionary<FXRenderTextureType, float>();

        private FXRenderTextureType[] _supportedTypes;

        private void Start()
        {
            var cameraRenderTexture = Get<Camera>().targetTexture;
            
            _textures = Finds<FXRenderTexture>();
            _textures.ForEach(t =>
            {
                t.RenderTexture = cameraRenderTexture; 
                t.transform.Off();
            });

            _particles = ChildrenGets<FXRenderTextureParticle>();
            _particles.ForEach(p => p.transform.Off());
            
            _supportedTypes = _particles.Select(p => p.type).ToArray();

            Debug.Log(_supportedTypes.Length);
        }

        public bool HasType(FXRenderTextureType type) => _supportedTypes.Contains(type);

        [Button("Play random")]
        public void PlayRandom()
        {
            PlayFX(_supportedTypes.GetRandomElement());
        }
        
        /// <summary>
        /// Plays FX, if last fx was not finished we reactivate it (play next one immediately)
        /// </summary>
        public async void PlayFX(FXRenderTextureType type)
        {
            Debug.Log("Start");
            
            if (!HasType(type))
                throw new Exception($"FXCamera does not contain type: {type.name}");
            
            var texture = _textures.FirstOrDefault(x => x.type == type);
            var particle = _particles.FirstOrDefault(x => x.type == type);

            if (texture == null || particle == null)
                throw new Exception("Some objects of this fx type was not prepared!");

            float lastTimeUsage = 0;

            texture.gameObject.Reactivate();
            particle.gameObject.Reactivate();

            _lastTimeUsages[type] = Time.time;

            await type.lifetime.Seconds();
            
            if (_lastTimeUsages.TryGetValue(type, out lastTimeUsage))
            {
                // New one effect was played
                if (Time.time < lastTimeUsage + type.lifetime)
                    return;
            }

            texture.transform.Off();
            particle.transform.Off(); 
        }
    }
}