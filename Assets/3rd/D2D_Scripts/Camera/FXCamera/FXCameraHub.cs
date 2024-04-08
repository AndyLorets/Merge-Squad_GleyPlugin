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
    public class FXCameraHub : SmartScript
    {
        private FXCamera[] _cameras;

        private void OnEnable()
        {
            _cameras = Finds<FXCamera>();
        }

        public void PlayFX(FXRenderTextureType type)
        {
            var supportedCamera = _cameras.FirstOrDefault(c => c.HasType(type));
            if (supportedCamera == null)
                throw new Exception($"There is no camera which supports type: {type.name}");
            
            supportedCamera.PlayFX(type);
        }
    }
}