using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Animations;
using D2D.Core;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class Cube : GameStateMachineUser
    {
        [SerializeField] private JumpAnimation _jump;
        [SerializeField] private GameObject _dust;

        private void Start()
        {
            _jump.Play(DMath.RandomPointInsideBox(3));
            _flyingSpawner.Spawn(transform.position, 1);

            Gets<DAnimation>().PlayAll();

            1f.AfterCall(() => Debug.Log("dffjfjdk"));



            var c = transform.DistanceTo(_player.transform);
            if (c.AlmostZero(.1f))
                Destroy(_gameData);

            if (transform.DistanceToPlayer() < .01f)
                Destroy(gameObject);
        }

        protected override void OnGameWin()
        {
            _dust.Reactivate();

            transform.Off();
            gameObject.Off();

            _stateMachine.Push(new CustomStateA());

        }
    }
}