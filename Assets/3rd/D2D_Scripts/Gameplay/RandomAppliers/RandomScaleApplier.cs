using D2D.Utilities;
using D2D.Utils;
using UnityEngine;

namespace D2D.Gameplay
{
    public class RandomScaleApplier : MonoBehaviour
    {
        [SerializeField] private Vector2 _scaleRange;
        [SerializeField] private bool _isRelative;

        private void Start()
        {
            var from = Vector3.one;

            if (_isRelative)
                from = transform.localScale;
            
            transform.localScale = from * _scaleRange.RandomFloat();
        }
    }
}
