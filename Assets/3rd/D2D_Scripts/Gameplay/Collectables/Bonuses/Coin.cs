using D2D.Animations;
using D2D.Databases;
using D2D.Utilities;
using D2D.Core;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Gameplay
{
    public class Coin : Collectable
    {
        [SerializeField] private int _cost = 1;
        [SerializeField] private GameObject _pickEffect;
        
        protected override void OnPick(GameObject owner)
        {
            // Add money bonus
            _db.Money.Value += _cost;
            
            // Some sparks, etc
            _pickEffect.On();
        }
    }
}