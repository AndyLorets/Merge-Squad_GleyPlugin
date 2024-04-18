using UnityEngine;
using UnityEngine.UI;
using YG;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    private int selectedButtonIndex = 0;
    private bool IsMobile => YandexGame.EnvironmentData.isMobile;

    void Start()
    {
        if (IsMobile) return;

        SelectButton(selectedButtonIndex); 
    }

    void Update()
    {
        if (IsMobile) return; 

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SelectPreviousButton();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SelectNextButton();
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            UseSelectedButton();
        }
    }

    void SelectButton(int index)
    {
        if (index >= 0 && index < buttons.Length)
        {
            buttons[index].Select(); 
            selectedButtonIndex = index;
        }
    }

    void SelectNextButton()
    {
        selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Length;
        SelectButton(selectedButtonIndex);
    }

    void SelectPreviousButton()
    {
        selectedButtonIndex--;
        if (selectedButtonIndex < 0)
        {
            selectedButtonIndex = buttons.Length - 1;
        }
        SelectButton(selectedButtonIndex);
    }

    void UseSelectedButton()
    {
        if (selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Length)
        {
            buttons[selectedButtonIndex].onClick.Invoke(); 
        }
    }
}
