/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;

namespace Amilious.Core.Editor.Modifiers {
    
    /// <summary>
    /// This modifier is used to dynamically set the label of a property.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicLabelAttribute))]
    public class DynamicLabelModifier : AmiliousPropertyModifier<DynamicLabelAttribute> {
        
        #region Protected Override Methods /////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden) {
            var prop = property.serializedObject.FindProperty(Attribute.StringOrField);
            if(prop == null){ 
                label.text = Attribute.StringOrField;
                return;
            }
            label.text = prop.stringValue ?? prop.objectReferenceValue.ToString();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}