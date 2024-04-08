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
using NaughtyAttributes;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;
using UpdateType = D2D.Common.UpdateType;

namespace D2D
{
    public class AnimatedZoomHub : SmartScript
    {
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private UpdateType _updateType = UpdateType.Update;

        [Header("Debug")] 
        [SerializeField] private float _to;
        [SerializeField] private float _duration;
        [SerializeField] private bool _playAtStart;

        private float Zoom
        {
            get => _transposer.m_CameraDistance;
            set => _transposer.m_CameraDistance = value;
        }

        private CinemachineFramingTransposer _transposer;
        private Transform _virtualBlendable;

        private void Start()
        {
            _transposer = Get<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineFramingTransposer>();

            _virtualBlendable = new GameObject("VirtualBlendable").transform;
            _virtualBlendable.transform.parent = transform;
            _virtualBlendable.transform.LocalPosition().x = Zoom;

            if (_playAtStart)
                AddRelativeZoomDebug();
        }
        
        private void Update()
        {
            if (_updateType == UpdateType.Update)
                UpdateZoom();
        }

        private void FixedUpdate()
        {
            if (_updateType == UpdateType.FixedUpdate)
                UpdateZoom();
        }

        private void LateUpdate()
        {
            if (_updateType == UpdateType.LateUpdate)
                UpdateZoom();
        }

        public void AddRelativeZoom(float to, float duration)
        {
            _virtualBlendable.DOBlendableLocalMoveBy(new Vector3(to, 0), duration).SetEase(_ease);
        }

        private void UpdateZoom()
        {
            Zoom = _virtualBlendable.localPosition.x;
        }

        [Button("Add relative zoom")]
        private void AddRelativeZoomDebug()
        {
            AddRelativeZoom(_to, _duration);
        }
    }
}