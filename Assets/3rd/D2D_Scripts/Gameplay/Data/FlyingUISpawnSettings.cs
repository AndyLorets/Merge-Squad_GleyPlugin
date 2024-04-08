using UnityEngine;

namespace D2D.Gameplay
{
    [CreateAssetMenu(fileName = "FlyingUISpawnSettings", menuName = "SO/FlyingUISpawnSettings", order = 0)]
    public class FlyingUISpawnSettings : ScriptableObject
    {
        // public float count = 1;
        public Vector2 amplitude;
        public Vector3 swift;
        public int moneyAddPerEach = 1;
        public bool needMoneyChange;
        public bool isRainAnimation;
    }
}