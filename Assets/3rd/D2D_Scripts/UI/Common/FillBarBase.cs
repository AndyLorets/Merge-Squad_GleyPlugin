using UnityEngine;
using UnityEngine.UI;

namespace D2D.UI
{
    public abstract class FillBarBase: MonoBehaviour
    {
        [SerializeField] private Image _image;

        protected virtual void Update()
        {
            _image.fillAmount = Calculate();
        }

        protected abstract float Calculate();
    }
}