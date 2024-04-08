using D2D.Utilities;
using D2D.Utils;
using TMPro;
using UnityEngine;

namespace D2D.UI
{
    [CreateAssetMenu(fileName = "UISettings", menuName = "SO/UISettings")]
    public class UISettings : SingletonData<UISettings>
    {
        [Header("Level windows")] 
        public float winWindowOpenDelay = 1;
        public float loseWindowOpenDelay = 1;
    }
}
