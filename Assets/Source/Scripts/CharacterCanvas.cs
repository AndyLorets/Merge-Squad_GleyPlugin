using D2D.Core;
using D2D.Gameplay;
using TMPro;
using UnityEngine;

public class CharacterCanvas : GameStateMachineUser
{
    public HealthBar HealthBar;
    public TextMeshProUGUI EvolutionText;

    private Camera currentCamera;
    private void Awake()
    {
        currentCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = currentCamera.transform.forward;
    }

    protected override void OnGameFinish()
    {
        gameObject.SetActive(false);
    }
}