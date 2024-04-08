using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        _audioManager = this;
    }
    public void PlayOneShot(AudioClip audioClip, float volume = 1f)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }
}