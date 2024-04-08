using D2D.Core;
using D2D.UI;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.UI
{
    public class Tutorial : GameStateMachineUser
    {
        private void Update()
        {
            if (Input.anyKey || DInput.IsMousePressing)
            {
                Confirm();
            }
        }

        private void Confirm()
        {
            _stateMachine.Push(new RunningState());
            Destroy(gameObject);
        }
    }
}