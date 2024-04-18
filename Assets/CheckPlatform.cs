using UnityEngine;
using YG; 

public class CheckPlatform : MonoBehaviour
{
    [SerializeField] private bool _isMobile;

    void Start()
    {
        gameObject.SetActive(false);

        Invoke(nameof(Show), 1f); 
    }

    private void Show()
    {
        bool isMobile = YandexGame.EnvironmentData.isMobile;

        if ((isMobile && _isMobile) || (!isMobile && !_isMobile))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
