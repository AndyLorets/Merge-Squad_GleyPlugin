using D2D.Utilities;
using UnityEngine;

namespace D2D
{
    public abstract class OnceObjectInteractor<T> : OnceObjectInteractorBase
        where T: Component
    {
        protected override void CheckInteraction(GameObject other)
        {
            if (other.Is<T>() && !isObjectInside && CanInteract(other))
            {
                isObjectInside = true;
                OnInteract(other.GetComponent<T>());
            }
        }

        public virtual bool CanInteract(GameObject other)
        {
            return true;
        }

        protected abstract void OnInteract(T target);
    }
}