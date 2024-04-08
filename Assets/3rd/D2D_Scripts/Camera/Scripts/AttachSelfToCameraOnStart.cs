using UnityEngine;

namespace D2D.Cameras
{
    public class AttachSelfToCameraOnStart : MonoBehaviour
    {
        [SerializeField] private bool _isZeroLocalPosition = true;
        [SerializeField] private Vector3 swift;

        private void Start()
        {
            var cam = FindObjectOfType<Camera>();
            transform.parent = cam.transform;

            if (_isZeroLocalPosition)
            {
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = swift;
            }
        }
    }
}