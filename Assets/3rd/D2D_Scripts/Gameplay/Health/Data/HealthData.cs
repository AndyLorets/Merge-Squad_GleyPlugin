using UnityEngine;

namespace D2D
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "ScriptableObjects/HealthData")]
    public class HealthData : ScriptableObject
    {
        public int maxPoints;
    }
}