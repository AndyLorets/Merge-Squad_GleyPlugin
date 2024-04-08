using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class FlyingUIPoolMember : PoolMember
    {
        private bool _isMoving;
        private Coroutine _cycle;
        private Tween _scale;
        private Tween _move;
        
        public void OnStartMove(Tweener scale, Tweener move, float duration, FlyingUIIcon coinIcon, FlyingUISpawnSettings settings)
        {
            if (_isMoving)
            {
                Debug.Log("You should increase number of ui coins in pool");
                
                StopCoroutine(_cycle);
                
                _scale?.Pause();
                _scale?.Kill();
                
                _move?.Pause();
                _move?.Kill();

                AddMoney();
            }

            _cycle = StartCoroutine(Cycle());
            IEnumerator Cycle()
            {
                _scale = scale;
                _move = move;
                _isMoving = true;

                yield return new WaitForSeconds(duration);

                _isMoving = false;

                coinIcon.Punch();

                AddMoney();
            }

            void AddMoney()
            {
                if (settings.needMoneyChange)
                    _db.Money.Value += settings.moneyAddPerEach;
            }
        }
    }
}