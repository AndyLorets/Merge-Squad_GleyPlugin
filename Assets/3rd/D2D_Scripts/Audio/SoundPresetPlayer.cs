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
    public class SoundPresetPlayer : SmartScript
    {
        public SoundPreset[] soundPresets;
        private AudioSource _audioSource;

        private void Start()
        {
            Reinit();
            
            if (_audioSource.playOnAwake)
                _audioSource.Play();
        }

        private void Reinit()
        {
            var preset = soundPresets.GetRandomElement();
            
            if (_audioSource == null)
                _audioSource = Get<AudioSource>();

            if (preset.clips.IsNullOrEmpty())
                return;
            
            _audioSource.clip = preset.clips.GetRandomElement();
            _audioSource.pitch = preset.pitch.RandomFloat();
            _audioSource.volume = preset.volume.RandomFloat();
        }

        public void Play()
        {
            Reinit();
            
            _audioSource.Play();
        }
    }
}