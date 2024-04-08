using D2D.Utilities;
using UnityEditor;

namespace D2D.Common
{
    [CustomEditor(typeof(Rotator))]
    [CanEditMultipleObjects]
    public class RotatorEditor : SuperEditor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            
            serializedObject.Update ();
            
            var rotator = (Rotator) target;
            ShowProperty(rotator.isRandom ? "_speed" : "speed");
            ShowProperty("_randomizeSign", "Randomize Sign");
            
            ShowProperty("_axis");
            ShowProperty("_updateType", "Update Type");
            ShowProperty("_useRigidbody", "Use Rigidbody");
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}