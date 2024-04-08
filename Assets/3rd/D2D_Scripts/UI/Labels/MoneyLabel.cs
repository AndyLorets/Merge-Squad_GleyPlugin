using D2D.Common;
using D2D.Databases;
using D2D.Utilities;
using UnityEngine;

namespace D2D.UI
{
    public class MoneyLabel : TrackableValueUI<float>
    {
        [SerializeField] private bool _useDollarPostfix = true;

        public override TrackableValue<float> FindTrackable() => 
            this.FindLazy<GameProgressionDatabase>().Money;

        protected override string ValueToText(float value)
        {
            return value.Round() + (_useDollarPostfix ? "$" : "");
        }
    }
}