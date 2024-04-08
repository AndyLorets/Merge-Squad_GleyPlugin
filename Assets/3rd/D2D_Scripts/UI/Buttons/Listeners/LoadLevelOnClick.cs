using D2D.Core;
using D2D.Utilities;
using D2D.Utils;

namespace D2D.UI
{
    public class LoadLevelOnClick : DButtonListener
    {
        public int levelNumber;
        
        protected override void OnClick()
        {
            var loader = this.FindLazy<SceneLoader>();
            loader.LoadLevel(levelNumber);
        }
    }
}