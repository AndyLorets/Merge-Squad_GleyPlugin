using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using D2D.Utilities;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using UnityEngine.UIElements;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class Testing_PushWin : GameStateMachineUser
    {
        private async UniTaskVoid Start()
        {
            await 1f.Seconds();
            
            _stateMachine.Push(new WinState());
        }
    }
}