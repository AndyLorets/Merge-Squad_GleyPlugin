using System.Collections;
using D2D.Core;
using D2D.UI;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.UI
{
    public class ReloadCurrentLevelOnClick : FinishScreenSceneLoader
    {
        protected override void LoadScene()
        {
            AdManager.ShowFullScreen();
            //Debug.Log("ShowAd");
            var loader = this.FindLazy<SceneLoader>();
            loader.ReloadCurrentScene();
        }
    }
}