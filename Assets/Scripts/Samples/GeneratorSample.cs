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
    public class GeneratorSample : MonoBehaviour
    {
        [Foldout("Easy")]
        [SerializeField] public float _number;
        
        #if UNITY_EDITOR
        [Button("Build")]
        public void Build()
        {
            Debug.Log("Build finished!");
        }
        #endif
    }
}