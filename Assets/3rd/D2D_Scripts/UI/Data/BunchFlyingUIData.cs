using UnityEngine;

namespace D2D.UI
{
    [CreateAssetMenu(fileName = "BunchFlyingUIData", menuName = "SO/BunchFlyingUIData")]
    public class BunchFlyingUIData : ScriptableObject
    {
        public Vector2 count;

        [Header("Bunch size up")]
        public float sizeUpDelay;
        public Vector2 sizeUpAmplitude;
        public Vector2 sizeUpDuration;
        public Vector2 sizeUpScale;

        [Header("Move to UI")] 
        public Vector2 afterMoveDuration;
        public Vector2 afterMoveScale;

        [Header("Haptic")] 
        public int hapticAmplitude;
        public int hapticDuration;
    }
}