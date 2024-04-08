using D2D.Core;
using D2D.Utilities;
using D2D.Utils;
using DG.Tweening;
using UnityEngine;


namespace D2D.UI
{
    public class GameFinishWindowsSwitcher : GameStateMachineUser
    {
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private GameObject _loseWindow;

        private void OnEnable()
        {
            _winWindow.Off();
            _loseWindow.Off();

            On<WinState>(ShowWinWindowAfterDelay);
            On<LoseState>(ShowLoseWindowAfterDelay);
        }
        
        private void ShowWinWindowAfterDelay()
        {
            float delay = UISettings.Instance.winWindowOpenDelay;
            _winWindow.On(after: delay);
        }
        private void ShowLoseWindowAfterDelay()
        {
            float delay = UISettings.Instance.loseWindowOpenDelay;
            _loseWindow.On(after: delay);
        }
    }
}