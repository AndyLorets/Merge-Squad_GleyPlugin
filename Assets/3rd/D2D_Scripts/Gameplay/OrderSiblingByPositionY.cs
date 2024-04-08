using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using NaughtyAttributes;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class OrderSiblingByPositionY : SmartScript
    {
        #if UNITY_EDITOR
        [Button("Order")]
        private void Order()
        {
            var sorted = transform.GetChildTransforms()
                .OrderByDescending(c => c.position.y).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                sorted[i].SetSiblingIndex(i);
            }
        }
        #endif
    }
}