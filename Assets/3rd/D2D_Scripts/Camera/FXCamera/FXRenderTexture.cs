using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using D2D;
using D2D.Gameplay;
using UnityEngine.UI;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public class FXRenderTexture : SmartScript
    {
        public FXRenderTextureType type;

        public Texture RenderTexture {set => Get<RawImage>().texture = value; }
    }
}