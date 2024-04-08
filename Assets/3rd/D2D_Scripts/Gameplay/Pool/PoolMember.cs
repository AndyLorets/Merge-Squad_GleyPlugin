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
    public class PoolMember : SmartScript
    {
        // [SerializeField] private PoolType _type;

        // public PoolType Type => _type;
        
        private Coroutine _hideRoutine;
        private bool _isHiding;

        public void HideAfter(Pool pool, float delay)
        {
            if (_isHiding)
            {
                // Debug.LogError("Size of pool to small! Increase it.");
                StopCoroutine(_hideRoutine);
            }

            _hideRoutine = StartCoroutine(Hiding(pool, delay));
            
            IEnumerator Hiding(Pool pool, float delay)
            {
                _isHiding = true;
                
                yield return new WaitForSeconds(delay);

                _isHiding = false;

                pool.HideEffect(this);
            }
        }
    }
}