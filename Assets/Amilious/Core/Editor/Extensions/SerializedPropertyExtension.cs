using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Amilious.Core.Editor.Extensions {
    
    public static class SerializedPropertyExtension {

        /// <summary>
        /// This method is used to get the attributes applied to the serialized property.
        /// </summary>
        /// <param name="property">The serialized property.</param>
        /// <param name="attributeType">The attribute type that you are looking for or null for all attributes.</param>
        /// <param name="inherit">If inherited attributes should be used.</param>
        /// <returns>An array of the attributes meeting the passed conditions.</returns>
        public static Attribute[] GetAttributes(this SerializedProperty property, Type attributeType = null, 
            bool inherit = true) {
            if(property == null) return Array.Empty<Attribute>();
            var type = property.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            foreach (var name in property.propertyPath.Split('.')) {
                fieldInfo = type.GetField(name, (BindingFlags)(-1));
                if (fieldInfo == null) {
                    propertyInfo = type.GetProperty(name, (BindingFlags)(-1));
                    if (propertyInfo == null) { return null; }
                    type = propertyInfo.PropertyType;
                } else { type = fieldInfo.FieldType; }
            }
            Attribute[] attributes;
            if (fieldInfo != null) {
                attributes = attributeType!=null ? 
                    fieldInfo.GetCustomAttributes(attributeType, inherit).Cast<Attribute>().ToArray() : 
                    fieldInfo.GetCustomAttributes(inherit).Cast<Attribute>().ToArray();
            } else if (propertyInfo != null) {
                attributes = attributeType!=null ? 
                    propertyInfo.GetCustomAttributes(attributeType, inherit).Cast<Attribute>().ToArray() : 
                    propertyInfo.GetCustomAttributes(inherit).Cast<Attribute>().ToArray();
            } else { return null; }
            return attributes;
        }

    }
}