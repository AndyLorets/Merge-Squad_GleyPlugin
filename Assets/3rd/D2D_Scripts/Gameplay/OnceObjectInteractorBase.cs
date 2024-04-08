using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using D2D.Utilities;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D
{
    public enum InteractionType {Trigger, Collision, Both}
    
    [RequireComponent(typeof(Rigidbody))]
    public abstract class OnceObjectInteractorBase : SmartScript
    {
        protected virtual InteractionType InteractionType => InteractionType.Both;

        protected bool isObjectInside;

        private void OnValidate()
        {
            var rb = Get<Rigidbody>();
            if (_coreData.objectInteractorValidation && rb.isKinematic)
            {
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (InteractionType == InteractionType.Both || InteractionType == InteractionType.Collision)
                CheckInteraction(other.gameObject);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (InteractionType == InteractionType.Both || InteractionType == InteractionType.Trigger)
                CheckInteraction(other.gameObject);
        }

        protected abstract void CheckInteraction(GameObject other);
    }
}