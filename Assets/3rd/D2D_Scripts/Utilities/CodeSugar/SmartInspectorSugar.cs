#if UNITY_EDITOR

using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using AV.Inspector.Runtime;
using AV.UITK;
using D2D;
using D2D.Core;
using D2D.Gameplay;
using D2D.Utils;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace D2D
{
    public static class SmartInspectorSugar
    {
        public static FluentElement<ToolbarButton> NewFastToolbarButton(this SmartInspector.EditorElement x, string caption, 
            Texture t, Color color, Action callback)
        {
            var button = x.NewButton(caption, t)
                .Style(Styles.Tab).TextColor(color).OnClick(callback);
            
            x.header.Add(button);

            if (CoreSettings.Instance.tools.hideInternalButtons)
                x.header.Get<FluentUITK.Space>().First().style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

            return null;
        }
        
        public static FluentElement<ToolbarButton> NewFastToolbarButton(this SmartInspector.EditorElement x, Texture t, Action a)
        {
            return x.NewFastToolbarButton(null, t, Color.red, a);
        }
        
        public static FluentElement<ToolbarButton> NewFastToolbarToggle(this SmartInspector.EditorElement x, Texture t, string propertyName)
        {
            return x.NewFastToolbarButton(null, t, Color.red, Toggle);
            
            void Toggle()
            {
                var serializedObject = x.editor.serializedObject;
                var p = serializedObject.FindProperty(propertyName);
                p.boolValue = !p.boolValue;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

#endif