using D2D.Gameplay;
using DG.Tweening;
using UnityEngine;

namespace D2D.Utilities
{
    public static class HealthSugar
    {
        /// <summary>
        /// Safe object destroy using health (if attached)
        /// </summary>
        public static void Kill(this GameObject target, GameObject killer, float delay = 0, bool checkHierarchy = false)
        {
            var health = target.GetComponent<Health>();

            // If obj has got health damage it by max value
            if (health != null)
            {
                DOVirtual.DelayedCall(delay, () => health.ApplyDamage(killer, health.MaxPoints));
            }
            else
            {
                // If obj hasn`t got health and disabled checking just simply destroy it
                if (!checkHierarchy)
                {
                    GameObject.Destroy(target, delay);
                }
                // If obj hasn`t got health but checking enabled search health in obj children 
                // or if there is no health search it in parent if no just simply destroy obj
                else
                {
                    health = target.GetComponentInChildren<Health>();

                    if (health != null)
                    {
                        DOVirtual.DelayedCall(delay, () => health.ApplyDamage(killer, health.MaxPoints));
                    }
                    else
                    {
                        health = target.GetComponentInParent<Health>();

                        if (health != null)
                        {
                            DOVirtual.DelayedCall(delay, () => health.ApplyDamage(killer, health.MaxPoints));
                        }
                        else
                        {
                            GameObject.Destroy(target, delay);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Auto object destroy (killer is actual object).
        /// </summary>
        public static void KillSelf(this GameObject target, float delay = 0, bool checkHierarchy = false)
        {
            Kill(target, target, delay, checkHierarchy);
        }
    }
}