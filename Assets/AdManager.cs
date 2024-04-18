using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AdManager : MonoBehaviour
{
    private void Start()
    {
        YandexGame.ErrorFullAdEvent += ErrorFullScreen;
        YandexGame.CloseFullAdEvent += CloseFullScreen; 
    }
    private void OnDestroy()
    {
        YandexGame.ErrorFullAdEvent -= ErrorFullScreen;
        YandexGame.CloseFullAdEvent -= CloseFullScreen;
    }
    public static void ShowFullScreen()
    {
        AudioListener.pause = true; 
        YandexGame.FullscreenShow();

        Debug.Log("ShowFullScreen");
    }
    private static void ErrorFullScreen()
    {
        AudioListener.pause = false;

        Debug.Log("ErrorFullScreen");
    }
    private static void CloseFullScreen()
    {
        AudioListener.pause = false;

        Debug.Log("CloseFullScreen");
    }
}
