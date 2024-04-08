using UltEvents;
using UnityEngine;

namespace D2D.Common
{
    public class DoActionOnTap : MonoBehaviour
    {
        [SerializeField] private UltEvent action;

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    action?.Invoke();
            }
        }
    }
}
