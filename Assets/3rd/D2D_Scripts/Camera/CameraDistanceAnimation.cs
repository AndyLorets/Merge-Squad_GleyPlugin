using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class CameraDistanceAnimation : SmartScript
    {
        [SerializeField] private float _startDelay;
        [SerializeField] private float _to;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        private CinemachineFramingTransposer _transposer;

        private async UniTaskVoid Start()
        {
            _transposer = Get<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

            if (_startDelay < 0)
                return;

            await _startDelay.Seconds();
            
            Animate();
        }

        private void Animate()
        {
            DOVirtual.Float(_transposer.m_CameraDistance, _to, _duration, v => _transposer.m_CameraDistance = v)
                .SetEase(_ease);
        }
    }
}