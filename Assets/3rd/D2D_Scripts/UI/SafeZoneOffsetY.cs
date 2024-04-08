using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Gameplay;
using D2D.Utilities;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class SafeZoneOffsetY : SmartScript
    {
        private void Start()
        {
            if (Screen.safeArea.size.y < Screen.height)
            {
                transform.position += new Vector3(0, _coreData.safeZoneOffset);
            }
        }
    }
}