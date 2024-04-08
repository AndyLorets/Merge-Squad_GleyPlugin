using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    public class RandomSwiftApplier : MonoBehaviour
    {
        [SerializeField] private float _amplitude;
        [SerializeField] private Vector3 _axes = Vector3.one;

        private void Start()
        {
            transform.position += DMath.RandomPointInsideBox(_amplitude).Multiply(_axes);
        }
    }
}
