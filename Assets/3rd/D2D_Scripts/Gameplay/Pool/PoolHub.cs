using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using UnityEngine.Analytics;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class PoolHub : SmartScript, ILazy
    {
        [SerializeField] private List<Pool> _manualSetupPools;
        
        private List<PoolType> _poolTypes = new List<PoolType>();

        private Dictionary<PoolType, Pool> _pools = 
            new Dictionary<PoolType, Pool>();

        private void Awake()
        {
            _poolHub = this;

            foreach (PoolType poolType in _poolTypes)
            {
                CreatePool(poolType);
            }

            foreach (Pool p in _manualSetupPools)
            {
                p.Init();

                _pools.Add(p.Type, p);
            }
        }

        private void CreatePool(PoolType poolType)
        {
            if (_pools.ContainsKey(poolType))
                return;
            
            var newPoolGameObject = new GameObject
            {
                transform =
                {
                    localPosition = Vector3.zero,
                    parent = transform
                },

                name = poolType.poolName
            };

            var newPool = newPoolGameObject.AddComponent<Pool>();
            newPool.Init(poolType);

            _pools.Add(poolType, newPool);
        }

        public GameObject Spawn(PoolType t, Vector3 position)
        {
            if (!_pools.ContainsKey(t))
            {
                _poolTypes.Add(t);
                CreatePool(t);
                
            }
            
            return _pools[t].Spawn(position);
        }
    }
}