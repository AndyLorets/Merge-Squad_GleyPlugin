using D2D.Common;
using D2D.Gameplay;
using D2D.UI;

namespace D2D
{
    public class LevelMultiplierLabel : TrackableValueUI<float>
    {
        public override TrackableValue<float> FindTrackable() => 
            FindObjectOfType<Level>().RewardMultiplier;

        protected override string ValueToText(float value)
        {
            return "x" + base.ValueToText(value);
        }
    }
}