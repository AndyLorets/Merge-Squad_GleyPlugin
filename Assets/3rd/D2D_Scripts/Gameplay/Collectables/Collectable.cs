using System;
using D2D.Utilities;
using UnityEngine;

namespace D2D.Gameplay
{
    /// <summary>
    /// For objects such as coins, gems and other instant effect bonuses and currency.
    /// If we want some grenades, cure poisons which we can use whenever we want
    /// we should use Item class. 
    /// </summary>
    public abstract class Collectable : MonoBehaviour, ICollectable
    {
        [SerializeField] protected float _lifeTimeAfterPick = 1;
        
        public bool IsPicked { get; private set; }

        public void Pick(GameObject owner)
        {
            if (IsPicked)
                throw new Exception("Already picked!");

            IsPicked = true;
            
            if (_lifeTimeAfterPick >= 0)
                gameObject.KillSelf(_lifeTimeAfterPick);
            
            OnPick(owner);
        }

        protected abstract void OnPick(GameObject owner);
    }
}