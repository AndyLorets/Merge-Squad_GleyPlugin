using System;
using System.Collections;
using D2D.Utilities;
using UnityEngine;
using UltEvents;
using D2D.Utils;

namespace D2D.Common
{
    /// <summary>
    /// Invokes the action after the delay
    /// </summary>
    public class DoActionOnStart : MonoBehaviour
    {
        [SerializeField] private UltEvent action;
        [SerializeField] public Vector2 delay;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delay.RandomFloat());

            action?.Invoke();
        }
    }
}
