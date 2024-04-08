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

namespace D2D.Utilities
{
    [CreateAssetMenu(fileName = "HapticSettings", menuName = "SO/HapticSettings")]
    public class HapticSettings : SingletonData<HapticSettings>
    {
        [Header("Light")] 
        public int lightAmplitude;
        public int lightDuration;
        
        [Header("Medium")]
        public int mediumAmplitude;
        public int mediumDuration;
        
        [Header("Heavy")]
        public int heavyAmplitude;
        public int heavyDuration;
    }
}