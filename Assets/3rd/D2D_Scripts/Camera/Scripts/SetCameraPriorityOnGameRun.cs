using Cinemachine;
using D2D.Core;
using UnityEngine;

public class SetCameraPriorityOnGameRun : GameStateMachineUser
{
    [SerializeField] private int _priority;

    protected override void OnGameRun()
    {
        GetComponent<CinemachineVirtualCamera>().Priority = _priority;
    }
}