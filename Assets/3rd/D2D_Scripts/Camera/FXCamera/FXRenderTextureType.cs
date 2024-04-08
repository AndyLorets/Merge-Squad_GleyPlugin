using D2D.Utilities;
using UnityEngine;

namespace D2D
{
    [CreateAssetMenu(fileName = "FXRenderTextureType", menuName = "SO/FXRenderTextureType", order = 0)]
    public class FXRenderTextureType : SingletonData<FXRenderTextureType>
    {
        public float lifetime;
    }
}