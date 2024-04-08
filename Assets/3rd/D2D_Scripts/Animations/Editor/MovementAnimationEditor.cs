using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Utils;
using UnityEditor;
using UnityEngine;
using static AV.Inspector.Runtime.SmartInspector;

namespace D2D.Animations
{
    [CustomEditor(typeof(MovementAnimation))]
    [CanEditMultipleObjects]
    public class MovementAnimationEditor : DAnimationEditor
    {
        protected override void ShowDefaultFields()
        {
            if (_target == null)
                return;
            
            var anim = _target as MovementAnimation;

            if (anim == null)
                return;

            ShowToProperty(anim);
            
            ShowDurationProperty();
            
            if (!anim.isEndPointModee)
                ShowProperty("_axis");

            if (_target.isAdvancedInfoVisible)
                return;
            
            ShowProperty("_loops");
        }

        protected override void ShowAdvancedInfo()
        {
            if (_target == null)
                return;
            
            var anim = _target as MovementAnimation;

            if (anim == null)
                return;

            /*if (CoreSettings.Instance.showRareTweenSettings)
                ShowProperty("_isFrom", "Is From");*/
            
            // ShowProperty("_isLocal", "Is local");
            
            // ShowProperty("_isHalf", "Is half");

            /*if (_target.isRandomnessSupported && _target.IsLooped)
                ShowProperty("_recalculateEveryLoop", "Recalc every loop");*/

            base.ShowAdvancedInfo();
        }

        protected override void ShowRandomFields()
        {
            ShowDefaultFields();
        }
        
        private void ShowToProperty(MovementAnimation anim)
        {
            if (!anim.isEndPointModee)
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

        /*[InitializeOnInspector]
        private static void Magic()
        {
            OnSetupEditorElement += x =>
            {
                if (!x.IsTarget<MovementAnimation>(out var target))
                    return;
                
                var icons = CoreSettings.Instance.tools.icons;
                x.NewFastToolbarToggle(icons.endPoint, nameof(target.isEndPointModee));
            };
        }*/
    }
}