using D2D.Utilities;
using UnityEngine;

namespace D2D.Animations
{
    [CreateAssetMenu(fileName = "DurationData", menuName = "DurationData", order = 0)]
    public class DurationData : ScriptableObject
    {
        [SerializeField] private Vector2 value;

        public Vector2 Value => value;
    }
}