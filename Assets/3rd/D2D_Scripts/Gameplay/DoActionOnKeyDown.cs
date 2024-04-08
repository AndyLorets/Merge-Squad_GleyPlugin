using UnityEngine;
using UltEvents;

namespace D2D.Common
{
    /// <summary>
    /// When player started pressing the key, invokes the action.
    /// Don`t use it in input, it is so laggy for it!
    /// </summary>
    public class DoActionOnKeyDown : MonoBehaviour
    {
        [SerializeField] private KeyCode keyCode;
        [SerializeField] private UltEvent action;

        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
                action?.Invoke();
        }
    }
}