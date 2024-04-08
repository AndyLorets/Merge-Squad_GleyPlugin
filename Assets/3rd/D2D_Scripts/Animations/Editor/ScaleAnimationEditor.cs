using AV.Inspector.Runtime;
using D2D.Utils;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;

namespace D2D.Animations
{
    [CustomEditor(typeof(ScaleAnimation))]
    [CanEditMultipleObjects]
    public class ScaleAnimationEditor : DAnimationEditor
    {
        protected override void ShowAdvancedInfo()
        {
            if (_target == null)
                return;
            
            var anim = _target as ScaleAnimation;

            if (anim == null)
                return;

            /*if (CoreSettings.Instance.showRareTweenSettings)
                ShowProperty("_isFrom", "Is From");*/

            base.ShowAdvancedInfo();
        }
    }
}