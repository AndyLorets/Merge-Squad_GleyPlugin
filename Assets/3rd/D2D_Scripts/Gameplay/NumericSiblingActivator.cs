using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class NumericSiblingActivator : SmartScript
    {
        [SerializeField] private bool _isFromTopToBottom;
        
        public int ActiveChildrenCount
        {
            set
            {
                var children = transform.GetChildTransforms();
                var n = value.Clamp(0, children.Count);

                children.ForEach(c => c.gameObject.Off());

                for (int i = _isFromTopToBottom ? 0 : 1; i < n; i++)
                {
                    if (_isFromTopToBottom)
                    {
                        children[i].gameObject.On();
                    }
                    else
                    {
                        children[children.Count-i].gameObject.On();
                    }
                }
            }
        }
    }
}