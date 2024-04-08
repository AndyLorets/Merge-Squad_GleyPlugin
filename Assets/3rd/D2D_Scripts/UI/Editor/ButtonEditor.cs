using D2D.Utilities;
using UnityEditor;

namespace D2D.UI
{
    [CustomEditor(typeof(DButton))]
    public class ButtonEditor : SuperEditor
    {
        public override void OnInspectorGUI()
        {
            BeginSerialization();
            
            ShowProperty("Clicked");

            var button = (DButton) target;
            if (button.needShowAdvancedProperties)
            {
                ShowProperty("PointerDown");
                ShowProperty("PointerUp");
                ShowProperty("MouseEnter");
                ShowProperty("MouseExit");

                Space();
                
                ShowProperty("_disabledAlpha");
                ShowProperty("_clickIsPointerDown");
            }
            
            EndSerialization();
        }
    }
}