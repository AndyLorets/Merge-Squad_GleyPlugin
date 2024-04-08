using System;
using UnityEngine;

namespace D2D.Gameplay
{
    /// <summary>
    /// Can be expanded to: pick only when space bar pressed (for items)
    /// </summary>
    public class CollectablesPicker : MonoBehaviour
    {
        public event Action<GameObject> Picked;
        
        /// <summary>
        /// Yeah, we could make calling Pick inside of Collectable with it own OnTriggerEnter
        /// method, but to avoid invalid OnTriggerEnter call order we do all stuff inside this
        /// small picker class.  
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var collectable = other.GetComponent<ICollectable>();
            if (collectable != null && !collectable.IsPicked)
            {
                collectable.Pick(owner: gameObject);
                Picked?.Invoke(gameObject);
            }
        }
    }
}