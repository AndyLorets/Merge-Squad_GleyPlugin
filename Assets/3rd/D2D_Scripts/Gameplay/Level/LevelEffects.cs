using System.Collections;
using System.Collections.Generic;
using D2D;
using D2D.Core;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

public class LevelEffects : GameStateMachineUser
{
    [SerializeField] private GameObject _winEffect;
    [SerializeField] private GameObject _loseEffect;

    protected override void OnGameWin()
    {
        _winEffect.On();
    }

    protected override void OnGameLose()
    {
        _loseEffect.On();
    }
}
