using UnityEngine;
using System;
using System.Collections;
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
    public class PlaySoundOnWin : GameStateMachineUser
    {
        private SoundPresetPlayer _soundPlayer;
        
        private void Awake()
        {
            _soundPlayer = Get<SoundPresetPlayer>(); 
            // _soundPlayer.soundPresets = new[] { _gameData.sounds.onWin };
        }

        protected override void OnGameRun()
        {
            _soundPlayer.Play();
        }
    }
}