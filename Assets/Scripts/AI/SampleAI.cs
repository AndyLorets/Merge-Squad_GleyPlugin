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

namespace D2D
{
    public class SampleAI : AIBase
    {
        protected override void InitStateMachine()
        {
            _fsm = new StateMachine();

            var idle = new LambdaState(tick: () => ConsoleProDebug.Watch("Idle", Time.deltaTime.ToString()), "Idle");
            var run = new LambdaState(tick: () => ConsoleProDebug.Watch("Run", Time.deltaTime.ToString()), "Run");
            var rush = new LambdaState(tick: () => ConsoleProDebug.Watch("Rush", Time.deltaTime.ToString()), "Rush");
            
            At(idle, run, () => Input.GetKeyDown(KeyCode.O));
            At(run, idle, () => Input.GetKeyDown(KeyCode.I));
            At(run, rush, () => Input.GetKeyDown(KeyCode.Y));
            At(rush, run, () => Input.GetKeyDown(KeyCode.Q));

            _fsm.SetState(idle);
        }
    }
}