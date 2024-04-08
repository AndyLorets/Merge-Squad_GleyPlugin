using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Utils;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;
using static AV.Inspector.Runtime.SmartInspector;

namespace D2D.Animations
{
    [CustomEditor(typeof(RotateAnimation))]
    [CanEditMultipleObjects]
    public class RotateAnimationEditor : DAnimationEditor
    {
        protected override bool IsFromSupported => false;
        
        protected override void ShowDefaultFields()
        {
            if (_target == null)
                return;

            var rotateAnimation = (RotateAnimation) _target;
            
            if (rotateAnimation == null)
                return;
            
            ShowToProperty(rotateAnimation);
            ShowDurationProperty();

            if (_target.isAdvancedInfoVisible)
                return;
            
            ShowLoopsProperty();
            ShowProperty("_ease");

            if (!rotateAnimation.isEndPointMode)
                ShowProperty("_axis");
        }
        
        private void ShowToProperty(RotateAnimation anim)
        {
            /*var to = anim.isRandomnessSupported ? "_to" : "to";
            ShowProperty(anim.isEndPointMode ? "_endPoint" : to,
                anim.isEndPointMode ? "End Point" : "To");*/
            
            if (!anim.isEndPointMode)
            {
                ShowProperty(anim.isRandomnessSupported ? "_to" : "to", "To");
                return;
            }

            if (anim.isRandomnessSupported)
            {
                ShowProperty("_endPoint", "To min");
                ShowProperty("_endPoint2", "To max");
            }
            else
            {
                ShowProperty("_endPoint", "To");
            }
        }

        protected override void ShowRandomFields()
        {
            ShowDefaultFields();
        }

        protected override void ShowAdvancedInfo()
        {
            var rotateAnimation = (RotateAnimation) _target;

            if (!rotateAnimation.isEndPointMode)
                ShowProperty("_axis");
            
            ShowProperty("isIncremental", "Is Incremental");
            // ShowProperty("_isLocal", "Is local");
            
            base.ShowAdvancedInfo();
        }
        
        /*[InitializeOnInspector]
        private static void Magic()
        {
            OnSetupEditorElement += x =>
            {
                if (!x.IsTarget<RotateAnimation>(out var target))
                    return;
                
                var icons = CoreSettings.Instance.tools.icons;
                x.NewFastToolbarToggle(icons.endPoint, nameof(target.isEndPointMode));
            };
        }*/
    }
}