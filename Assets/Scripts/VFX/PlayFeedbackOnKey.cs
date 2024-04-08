using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using MoreMountains.Feedbacks;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class PlayFeedbackOnKey : SmartScript
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Get<MMFeedbacks>().PlayFeedbacks();
            }
        }
    }
}