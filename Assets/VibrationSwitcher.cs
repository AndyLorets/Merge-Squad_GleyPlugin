using UnityEngine;
using UnityEngine.UI;
using D2D;

using static D2D.Utilities.CommonGameplayFacade;
using MoreMountains.NiceVibrations;

public class VibrationSwitcher : MonoBehaviour
{
    [SerializeField] private Button vibrationButton;
    [SerializeField] private Image cross;

    private const string Vibration = "Vibration";

    private void Awake()
    {
        vibrationButton.onClick.AddListener(SwitchVibration);
        UpdateCross();
    }
    private void Start()
    {
        bool isActive = PlayerPrefs.GetFloat(Vibration, 0) == 1;
        MMVibrationManager.SetHapticsActive(isActive);
    }

    private void SwitchVibration()
    {
        float value = PlayerPrefs.GetFloat(Vibration, 0);

        PlayerPrefs.SetFloat(Vibration, value == 0 ? 1 : 0);

        bool isActive = PlayerPrefs.GetFloat(Vibration, 0) == 1;
        MMVibrationManager.SetHapticsActive(isActive);

        UpdateCross();
    }
    private void UpdateCross()
    {
        cross.gameObject.SetActive(PlayerPrefs.GetFloat(Vibration, 0) == 1);
    }
}