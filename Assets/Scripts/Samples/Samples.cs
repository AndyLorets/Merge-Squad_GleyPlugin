using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using D2D.Utilities;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class Samples : SmartScript
    {
        // Finds, Gets
        // FSM
        // flying, camera, db
        // core, game sets
        // DRays
        // Math randoms, gameplay sugar off, on
        // Dotween, practicle use cases for anims

        private async UniTaskVoid Start()
        {
            _stateMachine.Push<WinState>();
            
            Get<Rigidbody>().NotNull();

            await 1f.Seconds();
            
            /*await transform.DOScale(Vector3.zero, 1f);
            await transform.DOScale(Vector3.one , 1f);
            await transform.DOMoveY(1f , 1f);
            await transform.DOScale(0 , 1f);*/

            /*_stateMachine.WasAny<WinState, LoseState>();
            _stateMachine.IsGameFinished;

            _stateMachine.Was<WinState>();
            _stateMachine.WasWin();

            _stateMachine.Was<LoseState>();
            _stateMachine.WasLose();*/
        }
    }
}