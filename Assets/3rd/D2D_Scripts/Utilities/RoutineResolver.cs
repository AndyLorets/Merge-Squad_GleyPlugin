using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class RoutineResolver : SmartScript, ILazy
    {
        private Dictionary<string, Tween> _tweens = new Dictionary<string, Tween>();
        private Dictionary<string, Coroutine> _routines = new Dictionary<string, Coroutine>();

        /// <summary>
        /// Stops previous if exists and runs current.
        /// </summary>
        public void Run(Tween tween, string id)
        {
            if (_tweens.ContainsKey(id))
            {
                _tweens[id].Pause();
                _tweens[id].Kill();
            }

            _tweens[id] = tween;
            _tweens[id].Play();
        }
        
        /// <summary>
        /// Stops previous if exists and runs current.
        /// </summary>
        public void Run(Coroutine routine, string id)
        {
            if (_routines.ContainsKey(id))
            {
                StopCoroutine(_routines[id]);
            }

            _routines[id] = routine;
        }

        public Tween GetTween(string id)
        {
            if (!_tweens.ContainsKey(id))
                return null;
            
            return _tweens[id];
        }
        
        public bool IsTweenPlaying(string id)
        {
            var tween = GetTween(id);
            return tween != null && tween.IsPlaying();
        }
        
        public Coroutine GetRoutine(string id)
        {
            if (!_routines.ContainsKey(id))
                return null;
            
            return _routines[id];
        }
        
        // Check using bool you have
        /*public bool IsRoutinePlaying(string id)
        {
            
        }*/
        
        public void StopTween(string id)
        {
            if (_tweens.ContainsKey(id))
            {
                _tweens[id].Pause();
                _tweens[id].Kill();
            }
        }
        
        public void StopRoutine(string id)
        {
            if (_routines.ContainsKey(id))
            {
                StopCoroutine(_routines[id]);
            }
        }
    }
}