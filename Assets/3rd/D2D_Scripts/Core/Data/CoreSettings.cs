using System;
using D2D.Core;
using D2D.Utilities;
using UnityEditor;
using UnityEngine;
using D2D.Tools;
using D2D.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Utils
{
    /// <summary>
    /// Contains all system game data.
    /// A bit a lot of responsibilities you might say, but creating data class
    /// for every single system it is not so handful too
    /// </summary>
    // [CreateAssetMenu(fileName = "CoreSettings", menuName = "SO/CoreSettings")]
    public class CoreSettings : SingletonData<CoreSettings>
    {
        public ProductionData production;
        
        [Header("Scenes")] 
            // public string levelScenePrefix = "Level_";
            public float nextLevelLoadDelay;
            public bool clearTweensOnSceneExit = true;
            // public Vector2 loopRangeLevel = new Vector2(1, 3);

        [Header("Code")]
            public bool lazyRuntimeCreationEnabled = true;
            public bool raycastDebugPauseEnabled;
            public bool allocationEnabled = true;
            public bool objectInteractorValidation = true;
            
            [Tooltip("Use only if you have a tons of game states, subscribers")]
            public bool stateMachineOptimizedMode;

        [Header("UI")] 
            public FlyingUISettings flyingUISettings;
            public BunchFlyingUIData defaultBunchData;
            public int safeZoneOffset = -90;
        
        [Header("Gameplay")] 
            public WinReward winReward;
        
        [Space]
        
        public ToolsSettings tools;

        #if UNITY_EDITOR

        [Button("ApplyProductionData")]
        public void ApplyProductionData()
        {
            var production = Instance.production;
            if (production.appName == "")
                return;
            
            // PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new []{production.icon});
            PlayerSettings.productName = production.appName;
            PlayerSettings.companyName = production.companyName;
            // PlayerSettings.keyaliasPass = production.keystorePassword;
            PlayerSettings.Android.keyaliasName = "d2d";
            PlayerSettings.keystorePass = "dd22dd88392073492!!@@**((";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, production.className);
            PlayerSettings.Android.bundleVersionCode = production.version;
            PlayerSettings.bundleVersion = "0." + production.version;
        }
        
        
        
        

        private void OnValidate()
        {
            if (defaultBunchData == null)
                defaultBunchData = GetAllInstances<BunchFlyingUIData>()[0];
        }
        
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for(int i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
 
        }
        
#endif
    }
    
    [Serializable]
    public class WinReward
    {
        [Serializable]
        public class RewardChunk
        {
            public int rewardIncrease;
            public int fromLevelNumber;
        }

        [SerializeField] private RewardChunk[] _chunks = new RewardChunk[]{new RewardChunk(){rewardIncrease = 50, fromLevelNumber = 0}};
        [Tooltip("Basically reward offset (zero level reward)")]
        [SerializeField] private float _startOffset = 50;

        public float GetRewardForCurrentLevel(int level = -1)
        {
            var passedLevels = _db.PassedLevels.Value;
            /*if (_stateMachine.Was<WinState>())
                passedLevels--;*/

            if (level != -1)
                passedLevels = level;

            float reward = _startOffset;
                
            for (int i = 1; i <= passedLevels; i++)
            {
                int increase = 0;
                foreach (RewardChunk c in _chunks)
                {
                    if (i >= c.fromLevelNumber)
                        increase = c.rewardIncrease;
                }

                reward += increase;
            }

            return reward;
        }
    }
    
    [Serializable]
    public class FlyingUISettings
    {
        public Vector2 claimCount;
        public float updateMoneyDbDelay;
        
        [Space]
        
        public float startScale;
        public float endScale;
        public float animationsDuration;
        public Ease animationEase;

        [Space]
        
        public Vector2 rainAmplitude;
    }
}