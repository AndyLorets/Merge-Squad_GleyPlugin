#if UNITY_EDITOR
using System;
using System.Linq.Expressions;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
namespace AV.UITK
{
    public partial class FluentElement<T>
    {
        // TODO: Challenge! Try to achieve complex binding with InspectorPrefs.PatchesTable 
        // TODO: SerializedProperty wrapper with linq-like deep iteration, i.e. Where("key", x => x.stringValue == patch.prefKey)
        
        public FluentElement<T> Bind(string bindingPath)
        {
            if (x is BindableElement bindable)
                bindable.bindingPath = bindingPath;
            else
                throw new Exception($"{x.name} is not a BindableElement. Can't assign '{bindingPath}' as binding path.");
            return x;
        }
        
        /// <summary> Retrieves full path of a source member and sets it as a binding path. </summary>
        public FluentElement<T> Bind<TSource>(Expression<Func<TSource, object>> member)
        {
            var fullPath = FluentUITK.GetMemberPath(member);

            Bind(fullPath); return x;
        }

        public FluentElement<T> Bind(SerializedProperty property)
        {
            Bind(property.propertyPath); return x;
        }
        
        public FluentElement<T> Bind(SerializedObject serializedObject) { x.Bind(serializedObject); return x; }
        
        public void Unbind() => x.Unbind();
        
        
        public FluentElement<PropertyField> NewField(SerializedProperty property, string label = null)
        {
            return new PropertyField(property) { label = label };
        }

        public FluentElement<PropertyField> NewField(string bindingPath, string label = null)
        {
            return new PropertyField { bindingPath = bindingPath, label = label };
        }
        
        public FluentElement<PropertyField> NewField<TSource>(Expression<Func<TSource, object>> member, string label = null)
        {
            var bindingPath = FluentUITK.GetMemberPath(member);
            
            return new PropertyField { bindingPath = bindingPath, label = label };
        }
    }
}
#endif
