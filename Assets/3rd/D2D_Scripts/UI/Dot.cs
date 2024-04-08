using System;
using D2D.Utilities;
using UnityEngine;

namespace D2D.UI
{
    public class Dot : MonoBehaviour
    {
        [SerializeField] private GameObject _onBody;
        [SerializeField] private GameObject _offBody;

        private float _timeSinceStart;

        private void Start()
        {
            _timeSinceStart = Time.time;
        }

        public bool IsFilled
        {
            set
            {
                _onBody.SetActive(value);
                _offBody.SetActive(!value);
                
                if (Time.time - _timeSinceStart < .1f)
                    return;

                if (value)
                {
                    _onBody.transform.PunchUI();
                }
                else
                {
                    _offBody.transform.PunchUI();
                }
            }
        }
    }
}