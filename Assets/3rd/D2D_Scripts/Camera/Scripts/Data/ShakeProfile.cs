using UnityEngine;

namespace D2D.Cameras
{
    [CreateAssetMenu(fileName = "ShakeProfile", menuName = "SO/ShakeProfile")]
    public class ShakeProfile : ScriptableObject
    {
        public float frequency;
        public float intensity;
        public float time;

        [Space(10)] 
        
        public float delay;
    }
}