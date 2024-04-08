using System.Collections;
using Cinemachine;
using UnityEngine;

public class SetCameraPriorityAfter : MonoBehaviour
{
    [SerializeField] private int _priority;
    [SerializeField] private float _after;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_after);

        GetComponent<CinemachineVirtualCamera>().Priority = _priority;
    }
}
