using D2D.Gameplay;
using UnityEngine;

namespace D2D
{
    public class InstantCureBonus : Collectable
    {
        [SerializeField] private float _healPoints;
        
        protected override void OnPick(GameObject owner)
        {
            owner.GetComponent<Health>().Heal(_healPoints);
        }
    }
}