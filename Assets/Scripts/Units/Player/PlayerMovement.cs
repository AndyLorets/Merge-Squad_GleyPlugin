using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Animancer;
using D2D;
using D2D.Animations;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class PlayerMovement : GameStateMachineUser
    {
        [SerializeField] private float _speed;
        
        [HideInInspector] public bool _canMove = true;

        private void Update()
        {
            if (!_canMove)
                return;

            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        protected override void OnGameLose()
        {
            _canMove = false;
        }
    }
}