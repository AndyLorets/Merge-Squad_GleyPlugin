using D2D.Utilities;
using UnityEngine;
using D2D.Utils;

namespace D2D.Common
{
    /// <summary>
    /// Destroys the gameObject after given delay. If delay is zero it do nothing
    /// </summary>
    public class Lifetimer : MonoBehaviour
    {
        [SerializeField] private Vector2 lifetime = Vector2.one;

        protected float CalculatedLifetime { get; private set; }

        private void OnEnable()
        {
            if (lifetime == Vector2.zero)
                return;

            CalculatedLifetime = lifetime.RandomFloat();
            
            gameObject.KillSelf(lifetime.RandomFloat());
        }
    }
}
