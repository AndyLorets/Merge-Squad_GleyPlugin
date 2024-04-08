using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using D2D;
using D2D.Animations;
using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.UI;
using D2D.Utilities;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.UI;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class FlyingUISpawner : SmartScript
    {
        [SerializeField] private Pool _flyingUIPool;
        [SerializeField] private FlyingUISpawnSettings _defaultSpawnSettings;

        private Camera _camera;
        
        private FlyingUIIcon _coinIcon;
        private PunchAnimation _coinIconPunch;

        private void Awake()
        {
            _camera = Find<Camera>();
            
            _coinIcon = Find<FlyingUIIcon>();
            _coinIconPunch = _coinIcon.Get<PunchAnimation>();
        }

        public async UniTaskVoid SpawnBunch(Vector3 screenPoint, Action onComplete)
        {
            var data = _coreData.defaultBunchData;
            var count = data.count.RandomInt();
            var angleSwift = 180 / count;
            
            DHaptic.Haptic(data.hapticDuration, data.hapticAmplitude);

            for (int i = 0; i < count; i++)
            {
                MakeAnimatedBunchFly(screenPoint, angleSwift, i, data);
            }

            await (data.sizeUpDuration.Max() + data.afterMoveDuration.Max()).Seconds();
            
            onComplete?.Invoke();
        }

        private async UniTask MakeAnimatedBunchFly(Vector3 screenPoint, float angleSwift, int i, BunchFlyingUIData data)
        {
            var fly = _flyingUIPool.Spawn(screenPoint).transform;
            var angle = angleSwift * i;
            // var to = new Vector3(angleSwift * i, angleSwift * i, 0).AngleToVector(transform).normalized * ;
            var to = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * data.sizeUpAmplitude.RandomFloat();
            var d = data.sizeUpDuration.RandomFloat();

            fly.transform.DOScale(data.sizeUpScale.RandomFloat(), d).SineEase();
            fly.transform.DOMove(to, d).SetRelative().SineEase();
            await d.Seconds();

            d = data.afterMoveDuration.RandomFloat();
            fly.transform.DOScale(data.afterMoveScale.RandomFloat(), d).SineEase();
            to = _coinIcon.transform.position;
            to.z = fly.transform.position.z;
            fly.transform.DOMove(to, d).SineEase();

            await d.Seconds();
            
            _coinIconPunch.Restart();
            _flyingUIPool.HideEffect(fly.Get<PoolMember>()); 
            DHaptic.Haptic(data.hapticDuration, data.hapticAmplitude);
        }

        public void Spawn(Vector3 from, int count, bool isPositionUI = false, FlyingUISpawnSettings settings = null)
        {
            for (int i = 0; i < count; i++)
                Spawn(from, isPositionUI, settings);
            
            _coreData.flyingUISettings.updateMoneyDbDelay.AfterCall(() => _db.Money.Value += count);
        }

        public void Spawn(Vector3 from, bool isPositionUI = false, FlyingUISpawnSettings settings = null)
        {
            if (settings == null)
                settings = _defaultSpawnSettings;
            
            
            var newFlyingUI = _flyingUIPool.Spawn(Vector3.zero).transform;

            var p = from;
            if (!isPositionUI)
                p = _camera.WorldToScreenPoint(from);

            var swift = settings.swift;
            var x = settings.amplitude.RandomFloat() * DMath.RandomSign();
            var y = settings.amplitude.RandomFloat() * DMath.RandomSign();
            swift.x += x;
            swift.y += y;

            newFlyingUI.position = p + swift;

            var data = _coreData.flyingUISettings;

            newFlyingUI.localScale = Vector3.one * data.startScale;
            Tweener scaleAnimation = newFlyingUI.DOScale(data.endScale, data.animationsDuration)
                .SetEase(data.animationEase);

            Tweener moveAnimation = !settings.isRainAnimation
                ? newFlyingUI.DOMove(_coinIcon.transform.position, data.animationsDuration)
                : newFlyingUI.DOMove(new Vector3(0, _coreData.flyingUISettings.rainAmplitude.RandomFloat()), data.animationsDuration).SetRelative();

            moveAnimation.SetEase(data.animationEase);

            newFlyingUI.Get<FlyingUIPoolMember>().
                OnStartMove(scaleAnimation, moveAnimation, data.animationsDuration, _coinIcon, settings);
        }
    }
}