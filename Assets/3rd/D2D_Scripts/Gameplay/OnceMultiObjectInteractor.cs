using D2D.Utilities;
using UnityEngine;

namespace D2D
{
    public abstract class OnceMultiObjectInteractor : OnceObjectInteractorBase
    {
        protected override void CheckInteraction(GameObject other)
        {
            if (!isObjectInside && CanInteract(other))
            {
                isObjectInside = true;
                OnInteract(other.gameObject);
            }
        }

        public abstract bool CanInteract(GameObject other);

        protected abstract void OnInteract(GameObject other);
    }
}