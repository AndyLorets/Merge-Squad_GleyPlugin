using D2D.Core;
using D2D.Databases;
using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    public class LastLevelSceneLoader : MonoBehaviour
    {
        private void Start()
        {
            var sceneLoader = this.FindLazy<SceneLoader>();
            var db = this.FindLazy<GameProgressionDatabase>();

            sceneLoader.LoadLevel(db.LastSceneNumber.Value);
        }
    }
}