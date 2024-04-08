using UnityEditor;

namespace D2D.Animations
{
    [CustomEditor(typeof(PunchAnimation))]
    [CanEditMultipleObjects]
    public class PunchAnimationEditor : DAnimationEditor
    {
        protected override bool IsFromSupported => false;
        
        protected override void ShowDefaultFields()
        {
            if (IsPunchUI())
            {
                ShowPunchUIProperty();
                return;
            }
            
            base.ShowDefaultFields();
            
            Space();

            ShowPunchFields();
        }

        protected override void ShowRandomFields()
        {
            if (IsPunchUI())
            {
                ShowPunchUIProperty();
                return;
            }
            
            base.ShowRandomFields();
            
            Space();
            
            ShowPunchFields();
        }

        protected override void ShowAdvancedInfo()
        {
            if (IsPunchUI())
                return;

            base.ShowAdvancedInfo();
        }

        private void ShowPunchFields()
        {
            ShowProperty("_vibratio");
            ShowProperty("_elasity");
            ShowPunchUIProperty();
        }

        private void ShowPunchUIProperty()
        {
            ShowProperty("_isPunchUI");
        }

        private bool IsPunchUI()
        {
            return serializedObject.FindProperty("_isPunchUI").boolValue;
        }
    }
}