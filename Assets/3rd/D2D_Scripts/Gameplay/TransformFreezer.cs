using UnityEngine;

namespace D2D.Common
{
    /// <summary>
    /// Laggy and deprecated stuff, not so good to use.
    /// Use rigidbody freeze for such things, not this script :)
    /// </summary>
    public class TransformFreezer : MonoBehaviour
    {
        [SerializeField] private bool freezeX;
        [SerializeField] private bool freezeY;
        [SerializeField] private bool freezeZ;
        [SerializeField] private bool freezeRotation;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private void Awake()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            HandlePosition();
            HandleRotation();
        }

        private void HandlePosition()
        {
            Vector3 position = transform.position;

            if (freezeX)
                position.x = initialPosition.x;

            if (freezeY)
                position.y = initialPosition.y;

            if (freezeZ)
                position.z = initialPosition.z;

            transform.position = position;
        }

        private void HandleRotation()
        {
            if (freezeRotation)
                transform.rotation = initialRotation;
        }
    }
}