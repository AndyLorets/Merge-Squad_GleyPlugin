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
    public class AxisMover : SmartScript
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _axis;

        [HideInInspector] public bool canMove = true;

        private void Update()
        {
            if (!canMove)
                return;

            transform.position += _axis * _speed * Time.deltaTime;
        }
    }
}