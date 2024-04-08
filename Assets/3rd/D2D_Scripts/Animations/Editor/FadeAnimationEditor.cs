using AV.Inspector.Runtime;
using D2D.Core;
using DG.Tweening.Plugins.Options;
using UnityEditor;
using UnityEngine;

namespace D2D.Animations
{
    [CustomEditor(typeof(FadeAnimation))]
    [CanEditMultipleObjects]
    public class FadeAnimationEditor : DAnimationEditor
    {
        protected override void ShowDefaultFields()
        {
            base.ShowDefaultFields();
            
            // ShowProperty("_isFrom", "Is From");
        }
    }
}