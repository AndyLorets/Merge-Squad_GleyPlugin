using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using NaughtyAttributes;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class Pool : SmartScript
    {
        [Tooltip("Leave it empty if it is not manual setup pool, like for UI")]
        [SerializeField] private PoolType _poolType;

        public PoolType Type => _poolType;

        private Queue<PoolMember> _instances = new Queue<PoolMember>();

        // private bool _isInited;
        
        /*private Dictionary<PoolMemberType, Queue<PoolMember>> _poolMemberInstances = 
            new Dictionary<PoolMemberType, Queue<PoolMember>>;*/

        /*private void Awake()
        {
            Init();
        }*/

        public void Init(PoolType t = null)
        {
            /*if (_isInited)
                return;*/

            // _isInited = true;

            
            if (t != null)
                _poolType = t;

            FillPoolWithEmpties();
        }

        private void FillPoolWithEmpties()
        {
            for (var i = 0; i < _poolType.size; i++)
            {
                SpawnPoolMember();
            }
        }

        private void SpawnPoolMember()
        {
            var newEffect = Instantiate(_poolType.prefab, transform.position, Quaternion.identity, transform);
            newEffect.gameObject.Off();
            _instances.Enqueue(newEffect);
        }

        private void Update()
        {
            var activeCount = _instances.Count(c => !c.gameObject.activeSelf);
            var allCount = _poolType.size;
            ConsoleProDebug.Watch("Pool objects affected", activeCount + " / " + allCount);
        }

        public GameObject Spawn(Vector3 position)
        {
            if (_instances.Count == 0)
            {
                SpawnPoolMember();
            }
            
            var effect = _instances.Dequeue();
            _instances.Enqueue(effect);

            effect.DOKill();
            effect.transform.position = position;
            effect.gameObject.On();
            
            effect.HideAfter(this, _poolType.lifetime);
            
            return effect.gameObject;
        }

        public void HideEffect(PoolMember effectToHide)
        {
            effectToHide.gameObject.Off();
        }
    }
}