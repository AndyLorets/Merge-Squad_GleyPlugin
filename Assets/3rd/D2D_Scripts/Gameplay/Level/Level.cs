using System;
using System.Linq;
using D2D.Common;
using UnityEngine;
using D2D.Core;
using D2D.Databases;
using D2D.Utilities;
using D2D.Utils;
using UnityEditor;
using UnityEngine.SceneManagement;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Gameplay
{
    /// <summary>
    /// Controls db (in case of level stuff)
    /// and contains info about this level scene (_sceneNumber_)
    /// </summary>
    public class Level : GameStateMachineUser
    {
        [Tooltip("Use -1 for non-level scene like menu, etc")]
        [SerializeField] private int _sceneNumber = -1;

        public int SceneNumber => _sceneNumber;
        
        private bool IsLast => _sceneNumber == SceneLoader.CountOfLevelsInGame;

        public float Reward => 
            CoreSettings.Instance.winReward.GetRewardForCurrentLevel() * RewardMultiplier.Value;

        public readonly TrackableValue<float> RewardMultiplier = new TrackableValue<float>(1f);
        
        public int GetNextSceneNumber()
        {
            // gameObject.EditorInstantiate()
        
            int nextSceneNumber = SceneNumber + 1;
            
            if (IsLast)
            {
                // nextSceneNumber = CoreSettings.Instance.loopRangeLevel.RandomInt();
            }
            
            return nextSceneNumber;
        }

        #region AutoDetection

        public bool IsValid => ActiveSceneNumber != -1;

        private int ActiveSceneNumber
        {
            get
            {
                var activeSceneName = SceneManager.GetActiveScene().name;
                var splits = activeSceneName.Split('_');

                if (int.TryParse(splits.Last(), out int n))
                    return n;

                return -1;
            }
        }

        private string ActiveScenePath => 
            SceneManager.GetActiveScene().path;

        #if UNITY_EDITOR

        private void OnValidate()
        {
            AutoDetect();
        }

        public void AutoDetect()
        {
            EditorUtility.SetDirty(this);

            if (ActiveSceneNumber == -1)
                return;
                // throw new Exception("You have level script on non-level scene! Remove it.");

            _sceneNumber = ActiveSceneNumber;

            // if (_coreData.loopRangeLevel.y < _sceneNumber)
               // _coreData.loopRangeLevel.y = _sceneNumber;

            AddSceneToBuildSettings();
        }

        private void AddSceneToBuildSettings()
        {
            var buildScenes = EditorBuildSettings.scenes.ToList();

            var newBuildScene = new EditorBuildSettingsScene(ActiveScenePath, true);

            // Avoid duplicates
            for (var i = 0; i < buildScenes.Count; i++)
            {
                var current = SceneManager.GetActiveScene();
                var currentPath = current.path + current.name;
                var other = SceneManager.GetSceneByBuildIndex(i);
                var otherPath = other.path + other.name;
                if (currentPath == otherPath)
                    return;
            }
            
            buildScenes.Add(newBuildScene);
            
            EditorBuildSettings.scenes = buildScenes.ToArray();
        }

        #endif

        #endregion
    }
}