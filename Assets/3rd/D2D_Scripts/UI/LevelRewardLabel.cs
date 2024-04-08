using D2D.Common;
using D2D.Gameplay;
using D2D.UI;
using D2D.Utilities;
using UnityEngine;

namespace D2D.UI
{
    public class LevelRewardLabel : TrackableValueUI<float>
    {
        [SerializeField] private string _preText;

        public override TrackableValue<float> FindTrackable() => 
            new TrackableValue<float>(FindObjectOfType<Level>().Reward);

        protected override string ValueToText(float value) => 
            _preText + value.Round();

        protected override string FloatToText(float value) =>
            _preText + value.Round();
    }
}