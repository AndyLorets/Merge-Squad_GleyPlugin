using UnityEngine;

namespace D2D.Gameplay
{
    /// <summary>
    /// A base class for all collectables (coins, gems, weapons, ruins, ammo, cures, etc). 
    /// </summary>
    public interface ICollectable
    {
        public bool IsPicked { get; }

        public void Pick(GameObject owner);
    }
}