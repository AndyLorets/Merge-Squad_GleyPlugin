using UnityEngine;

namespace D2D.UI
{
    public class CanvasChild : MonoBehaviour
    {
        private void Start()
        {
            var canvas = FindObjectOfType<Canvas>();
            transform.parent = canvas.transform;
        }
    }
}
