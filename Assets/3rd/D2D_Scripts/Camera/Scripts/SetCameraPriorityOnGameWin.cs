using Cinemachine;
using D2D.Core;
using UnityEngine;

public class SetCameraPriorityOnGameWin : GameStateMachineUser
{
    [SerializeField] private int _priority;

    protected override void OnGameWin()
    {
        GetComponent<CinemachineVirtualCamera>().Priority = _priority;
    }
}