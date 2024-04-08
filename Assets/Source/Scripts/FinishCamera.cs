using Cinemachine;
using D2D.Core;
using DG.Tweening;
using UnityEngine;

public class FinishCamera : GameStateMachineUser
{
    [SerializeField] private float rotateSpeed = 4f;

    private Vector3 rotateVector = new Vector3(0, 60, 0);

    protected override void OnGameWin()
    {
        base.OnGameWin();

        transform.DOLocalRotate(transform.rotation.eulerAngles + rotateVector, rotateSpeed).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear);

        SetFinishCamera();
    }

    private void SetFinishCamera()
    {
        Get<CinemachineVirtualCamera>().m_Priority = 100;
    }
}