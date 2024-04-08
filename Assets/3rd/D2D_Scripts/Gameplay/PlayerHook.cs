using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D;
using D2D.Common;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utilities;
using DG.Tweening;

namespace D2D
{
    public class PlayerHook : TransformHook
    {
        protected override void Start()
        {
            base.Start();
            Target = FindObjectOfType<Player>().transform;
        }
    }
}