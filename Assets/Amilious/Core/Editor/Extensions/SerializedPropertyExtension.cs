using System;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Extensions {
    
    public static class SerializedPropertyExtension {

        /// <summary>
        /// This method is used to get the attributes applied to the serialized property.
        /// </summary>
        /// <param name="property">The serialized property.</param>
        /// <param name="attributeType">The attribute type that you are looking for or null for all attributes.</param>
        /// <param name="inherit">If inherited attributes should be used.</param>
        /// <returns>An array of the attributes meeting the passed conditions.</returns>
        public static IEnumerable<Attribute> GetAttributes(this SerializedProperty property, Type attributeType = null, 
            bool inherit = true) {
            if(property == null) return Array.Empty<Attribute>();
            var type = property.serializedObject.targetObject.GetType();
            MemberInfo memberInfo = null;
            while(type != null && memberInfo == null) {
                memberInfo = type.GetField(property.name, (BindingFlags)(-1)) as MemberInfo ?? 
                             type.GetProperty(property.name, (BindingFlags)(-1));
                type = type.BaseType;
            }
            if(memberInfo==null) return Array.Empty<Attribute>();
            return attributeType!=null ? 
                memberInfo.GetCustomAttributes(attributeType, inherit).Cast<Attribute>() : 
                memberInfo.GetCustomAttributes(inherit).Cast<Attribute>();
        }

    }
}