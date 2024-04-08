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
    public class FastConfettiMaker : SmartScript
    {
        [SerializeField] private PoolType vfx;
        
        [Foldout("Numbers")] [SerializeField] private float _spawnDelay;
        [Foldout("Numbers")] [SerializeField] private float _spawnRange;

        private IEnumerator Start()
        {
            var poolHub = this.FindLazy<PoolHub>();
            
            while (enabled)
            {
                var p = transform.position + DMath.RandomPointInsideBox(_spawnRange);
                poolHub.Spawn(vfx, p);
                
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}