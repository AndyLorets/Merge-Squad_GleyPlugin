using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Utils;
using UnityEditor;

namespace D2D.Animations
{
    [CustomEditor(typeof(JumpAnimation))]
    [CanEditMultipleObjects]
    public class JumpAnimationEditor : DAnimationEditor
    {
        protected override bool IsFromSupported => false;
        
        protected override void ShowDefaultFields()
        {
            ShowTo();
            ShowDurationProperty();

            if (!_target.isAdvancedInfoVisible)
            {
                bool isRandom = _target.isRandomnessSupported;
                ShowProperty(isRandom ? "_power" : "power");
            }
        }

        protected override void ShowRandomFields()
        {
            ShowTo();
            ShowDurationProperty();
        }

        private void ShowTo()
        {
            if (_target.isRandomnessSupported)
            {
                ShowProperty("_endPoint", "To min");
                ShowProperty("_endPoint2", "To max");
            }
            else
            {
                ShowProperty("_endPoint", "To");
            }
        }

        protected override void ShowAdvancedInfo()
        {
            if (_target == null)
                return;
            
            var anim = _target as JumpAnimation;

            if (anim == null)
                return;
            
            bool isRandom = _target.isRandomnessSupported;
            
            ShowProperty(isRandom ? "_power" : "power");
            ShowProperty(isRandom ? "_steps" : "steps");
            // ShowProperty("_isLocal", "Is local");
            
            base.ShowAdvancedInfo();
        }
    }
}