using System;
using D2D.Core;
using D2D.Utilities;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class PlayWinAudioOnWin : GameStateMachineUser
    {
        [SerializeField] private AudioClip[] _clips;
        
        protected override void OnGameWin()
        {
            var a = Get<AudioSource>();
            
            if (!_clips.IsNullOrEmpty())
                a.clip = _clips.GetRandomElement();
                
            a.Play();
            // (_audioPool.Spawn(transform.position) as DAudio).Play(_gameData.winSound);
        }
    }
}