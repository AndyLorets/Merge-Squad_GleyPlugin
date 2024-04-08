using System.Collections;
using System.Collections.Generic;
using D2D.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace D2D.Common
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class FadeoutLifetimer : Lifetimer
    {
        [SerializeField] private float startFadeoutTime = .5f;
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(startFadeoutTime);

            GetComponent<MaskableGraphic>().DOFade(0, CalculatedLifetime - startFadeoutTime);
        }
    }
}