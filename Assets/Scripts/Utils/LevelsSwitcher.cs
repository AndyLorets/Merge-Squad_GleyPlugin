using System;
using System.Collections.Generic;
using D2D.Core;
using D2D.Databases;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    [DefaultExecutionOrder(-99)]
    public class LevelsSwitcher : GameStateMachineUser
    {
        [Tooltip("-1 for random")]
        [SerializeField] private int _debugCurrentLevel = -1;
        
        [Space]

        [SerializeField] private bool _isRandomOnLoop = true;
        
        [Tooltip("To skip for instance 1 level (on loop) put value to 2")]
        [SerializeField] private int _minRepeatLevel = 1;

        [Tooltip("Just be sure that all passed level switchers have different names")]
        [SerializeField] private string _uniqueContainerName = "Levels";

        private DataContainer<int> _lastLoadedLevelIndex;
        private DataContainer<bool> _isLastLevelWin;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_gameData.levels.IsNullOrEmpty())
                return;
            
            if (_debugCurrentLevel >= 0)
            {
                Instantiate(_gameData.levels[_debugCurrentLevel-1]);
                return;
            }

            var id = _uniqueContainerName;
            _lastLoadedLevelIndex = new DataContainer<int>("LastShuffledIndex" + id, -1);
            _isLastLevelWin = new DataContainer<bool>("IsLastLevelWin" + id, true);

            if (!_isLastLevelWin.Value && _lastLoadedLevelIndex.Value > -1)
            {
                Instantiate(_gameData.levels[_lastLoadedLevelIndex.Value]);
            }
            else
            {
                DefaultLoad();
            }

            _isLastLevelWin.Value = false;
        }

        private void DefaultLoad()
        {
            var levelIndex = _db.PassedLevels.Value;
            var index = levelIndex % _gameData.levels.Count;
            
            // If we run out of levels => do magic now (repeat levels, so player will not notice)
            if (levelIndex > _gameData.levels.Count-1)
            {
                int minRepeatLevelIndex = _minRepeatLevel - 1;
                index = minRepeatLevelIndex + (levelIndex) % (_gameData.levels.Count - minRepeatLevelIndex);
                
                if (_isRandomOnLoop)
                {
                    var min = _minRepeatLevel - 1;
                    var max = _gameData.levels.Count - 1;
                    var lastShuffledIndex = _lastLoadedLevelIndex.Value;
                    
                    List<int> indexes = new List<int>();
                    for (int l = min; l <= max; l++)
                    {
                        if (l != lastShuffledIndex)
                            indexes.Add(l);
                    }

                    index = indexes.GetRandomElement();
                }
            }

            _lastLoadedLevelIndex.Value = index;

            try
            {
                Instantiate(_gameData.levels[index]);
                // children[index].transform.On();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Instantiate(_gameData.levels.GetRandomElement());
                // children[0].transform.On();
            }
        }

        protected override void OnGameWin()
        {
            _isLastLevelWin.Value = true;
        }
    }
}