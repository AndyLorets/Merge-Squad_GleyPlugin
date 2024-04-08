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
    [CreateAssetMenu(fileName = "SoundPreset", menuName = "SO/SoundPreset")]
    public class SoundPreset : ScriptableObject
    {
        public AudioClip[] clips;
        public Vector2 pitch;
        public Vector2 volume;
    }
}