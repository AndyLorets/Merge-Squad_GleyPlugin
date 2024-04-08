using AV.Inspector.Runtime;
using D2D.Core;
using D2D.Utilities;
using D2D.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace D2D.Gameplay
{
    [CustomEditor(typeof(Health))]
    public class HealthEditor : SuperEditor
    {
        public override void OnInspectorGUI()
        {
            BeginSerialization();

            var health = (Health) target;

            // Show max points or data
            ShowProperty(health.isHealthDataMode ? "_healthData" : "_maxPoints", 
                health.isHealthDataMode ? "Data" : "Max Points");
            
            // Show current points
            if (Application.isPlaying)
                ReadOnlyLine("Current points", health.CurrentPoints.ToString());

            // Show health effects (if needed)
            if (health.particlesAreEnabled)
            {
                Space(10);
                ShowProperty("_hitEffect", "Hit Effect");
                ShowProperty("_deathEffect", "Death Effect");
                ShowProperty("_meshRenderer", "Mesh Renderer");
                ShowProperty("gradient", "Gradient");
            }
            
            ShowProperty("_isGrayFadeout", "Is Gray Fadeout");

            EndSerialization();
        }
        
        [InitializeOnInspector]
        private static void OnInspector()
        {
            SmartInspector.OnSetupEditorElement += x =>
            {
                if (!(x.target is Health target))
                    return;

                var icons = CoreSettings.Instance.tools.icons;
                x.NewFastToolbarToggle(icons.effects, nameof(target.particlesAreEnabled));
                x.NewFastToolbarToggle(icons.data, nameof(target.isHealthDataMode));
            };
        }
    }
}