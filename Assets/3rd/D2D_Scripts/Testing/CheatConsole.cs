using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;
using Sirenix.OdinInspector;

namespace D2D
{
    public class CheatConsole : SmartScript
    {
        [SerializeField] private float _money;
        [SerializeField] private int _levelsPassed;

        [Button("Update $$$")]
        private void SetMoney()
        {
            _db.Money.Value = _money;
        }
        
        [Button("Update levels")]
        private void LevelsPassed()
        {
            _db.PassedLevels.Value = _levelsPassed;
        }
        
        [Button("Win")]
        private void PushWin()
        {
            _stateMachine.Push<WinState>();
        }
        
        [Button("Lose")]
        private void PushLose()
        {
            _stateMachine.Push(new LoseState());
        }
        
        [Button("Reload")]
        private void ReloadLevel()
        {
            _sceneLoader.ReloadCurrentScene();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PushWin();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadLevel();
            }
        }
    }
}