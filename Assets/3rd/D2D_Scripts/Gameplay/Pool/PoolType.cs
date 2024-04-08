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
    [CreateAssetMenu(fileName = "PoolType", menuName = "SO/PoolType")]
    public class PoolType : ScriptableObject
    {
        public string poolName;
        public PoolMember prefab;
        public int size;
        public int lifetime;
    }
}