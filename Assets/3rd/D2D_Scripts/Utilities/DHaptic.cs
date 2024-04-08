using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using MoreMountains.NiceVibrations;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public static class DHaptic
    {
        private const float MinDelayBetweenHaptics = .3f;
        private static float _timeOfNextHaptic;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void ResetLastTime()
        {
            _timeOfNextHaptic = 0;
        }
        
        public static void HapticLight() => Haptic(_hapticData.lightDuration, _hapticData.lightAmplitude);

        public static void HapticMedium() => Haptic(_hapticData.mediumDuration, _hapticData.mediumAmplitude);
        
        public static void HapticHard() => Haptic(_hapticData.heavyDuration, _hapticData.heavyAmplitude);

        public static void Haptic(int d, int a)
        {
            if (Time.time >= _timeOfNextHaptic)
            {
                MMNVAndroid.AndroidVibrate(d, a);
                _timeOfNextHaptic = Time.time + MinDelayBetweenHaptics;
            }
        }
    }
}