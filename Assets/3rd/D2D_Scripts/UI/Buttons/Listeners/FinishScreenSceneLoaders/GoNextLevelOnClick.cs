using D2D.Core;
using D2D.Databases;
using D2D.Gameplay;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.UI
{
    public class GoNextLevelOnClick : FinishScreenSceneLoader
    {
        protected override void LoadScene()
        {
            _sceneLoader.ReloadCurrentScene();
            // _sceneLoader.LoadLevel(_level.GetNextSceneNumber());
        }
    }
}