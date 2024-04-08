using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Animations;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class ChildNumericRigidbodyHandler : SmartScript
    {
        [SerializeField] private bool _isFromTop;

        private Rigidbody[] _children;

        public int ChildCount => transform.childCount;

        private void Awake()
        {
            _children.ForEach(c => c.isKinematic = true);
        }

        public void SetNonKinematicCount(int value, Action<Rigidbody> predicate = null)
        {
            if (_children.IsNullOrEmpty())
                _children = ChildrenGets<Rigidbody>();

            var n = value.Clamp(0, _children.Length);

            // _children.ForEach(c => c.isKinematic = true);

            for (int i = _isFromTop ? 1 : 0; i < n; i++)
            {
                var child = _children[!_isFromTop ? i : _children.Length - i];

                /*if (child.isKinematic)
                {
                    child.transform.DOScale(0, .7f).SetDelay(2f);
                }*/

                predicate?.Invoke(child);

                if (child.isKinematic)
                    child.isKinematic = false;

            }
        }
    }
}