using UnityEngine;
using System;
using System.Collections;
using System.Linq;
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
    public class Testing_SubToWinAndDie : GameStateMachineUser
    {
        protected override void OnGameWin()
        {
            Debug.Log(gameObject);
            transform.DOMoveY(5, 1f).SetRelative();
        }
    }
}