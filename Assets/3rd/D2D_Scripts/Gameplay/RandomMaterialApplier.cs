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
    [RequireComponent(typeof(MeshRenderer))]
    public class RandomMaterialApplier : SmartScript
    {
        [SerializeField] private Material[] _materials;

        private void Start()
        {
            Get<MeshRenderer>().material = _materials.GetRandomElement();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                Get<MeshRenderer>().material = _materials.GetRandomElement();
        }
    }
}