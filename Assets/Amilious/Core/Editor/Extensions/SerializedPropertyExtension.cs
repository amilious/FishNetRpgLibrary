////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="SerializedProperty"/> class.
    /// </summary>
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

        /// <summary>
        /// This method is used to get the attributes applied to the serialized property.
        /// </summary>
        /// <param name="property">The serialized property.</param>
        /// <param name="inherit">If inherited attributes should be used.</param>
        /// <typeparam name="T">The attribute type that you are looking for.</typeparam>
        /// <returns>The attributes of the given type {T}.</returns>
        public static IEnumerable<T> GetAttributes<T>(this SerializedProperty property, bool inherit = true) where T : Attribute {
            return property.GetAttributes(typeof(T), inherit).Cast<T>();
        }
        
    }
}