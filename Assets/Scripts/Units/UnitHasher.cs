using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    /// <summary>
    /// Small optimization.
    /// </summary>
    public class UnitHasher : GameStateMachineUser, ILazy
    {
        public IEnumerable<Enemy> Enemies { get; private set; }

        private void OnEnable()
        {
            Cache();
        }

        private void Awake()
        {
            Cache();
        }

        private void Start()
        {
            Cache();
        }
        
        private void Update()
        {
            Cache();
        }

        private void Cache()
        {
            Enemies = FindObjectsOfType<Enemy>().Where(h => h != null);
        }
    }
}