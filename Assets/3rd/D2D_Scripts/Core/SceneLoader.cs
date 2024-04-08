using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using D2D.Utilities;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Core
{
    /// <summary>
    /// Responsible for scene loading, takes data from ScenesSettings.
    /// </summary>
    public class SceneLoader : MonoBehaviour, ILazy
    {
        // The nam of current scene.
        public static string CurrentSceneName => 
            SceneManager.GetActiveScene().name;
        
        /// <summary>
        /// Count of levels in game.
        /// </summary>
        public static int CountOfLevelsInGame => 
            SceneManager.sceneCountInBuildSettings - 1;
        
        // Use for it gsm.On<SceneLoading>();
        // public event Action SceneReloading;

        private bool _isSceneLoading;

        public void ReloadCurrentScene()
        {
            LoadScene(CurrentSceneName);
        }

        public async void LoadLevel(int levelNumber)
        {
            if (levelNumber < 1)
            {
                Debug.LogError("You tried to load level with number less than 1! I wll load level 1");
                levelNumber = 1;
            }

            if (levelNumber > CountOfLevelsInGame)
            {
                Debug.LogError("You tried to load level more than count of levels in game! I will load 1 level");
                levelNumber = 1;
            }

            // LoadScene(_coreData.levelScenePrefix + levelNumber);
        }

        private async void LoadScene(string sceneName)
        {
            if (_isSceneLoading)
                return;

            _isSceneLoading = true;

            if (_level == null)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
            
            _stateMachine.Push(new SceneLoading());

            if (_coreData.nextLevelLoadDelay > 0)
                await _coreData.nextLevelLoadDelay.Seconds();

            if (_coreData.clearTweensOnSceneExit)
                DOTween.Clear(true);
                
            SceneManager.LoadScene(sceneName);
        }
        
        /*private async void StartSceneLoading(string loadingSceneName)
        {
            if (FindObjectOfType<Level>() == null)
            {
                SceneManager.LoadScene(loadingSceneName);
                yield break;
            }
            
            _stateMachine.Push(new SceneLoading());

            if (_coreData.veryFirstNextLevelLoadDelay > 0)
                yield return new WaitForSeconds(_coreData.veryFirstNextLevelLoadDelay);

            if (_coreData.levelTransitionDuration > 0)
                yield return new WaitForSeconds(_coreData.levelTransitionDuration);

            if (_coreData.clearTweensOnSceneExit)
                DOTween.Clear(true);
                
            SceneManager.LoadScene(loadingSceneName);
        }*/
    }
}
