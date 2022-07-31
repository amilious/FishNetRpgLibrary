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

using UnityEngine;

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This is the base class for the <see cref="ShowIfAttribute"/> and the <see cref="HideIfAttribute"/>
    /// </summary>
    public abstract class ShowHideIf : AmiliousModifierAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The property to use as the condition.
        /// </summary>
        public string PropertyName { get; protected set; }

        /// <summary>
        /// If this value is true the comparison value was provided, otherwise it was not.
        /// </summary>
        protected bool SetValue { get; set; }
        
        /// <summary>
        /// This property contains the value that you want to compare to.
        /// </summary>
        protected object Value { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to validate a value.
        /// </summary>
        /// <param name="value">The value that you want to validate.</param>
        /// <typeparam name="T">The type of the value that you want to validate.</typeparam>
        /// <returns>True if the value is valid otherwise false.</returns>
        public bool Validate<T>(T value) {
            Debug.Log("reached 2");
            if(SetValue) return Value is T casted && casted.Equals(value);
            if(value is bool boolValue) return boolValue;
            if(default(T) == null) return value != null;
            return false;
        }

        /// <summary>
        /// This method is used to validate a value.
        /// </summary>
        /// <param name="value">The value that you want to validate.</param>
        /// <returns>True if the value is valid otherwise false.</returns>
        public bool Validate(object value) {
            Debug.Log("reached");
            if(SetValue) return value.Equals(Value);
            if(value is bool boolValue) return boolValue;
            return value != null;
        }

        /// <summary>
        /// This method is used to validate an enum value.
        /// </summary>
        /// <param name="index">The enum index.</param>
        /// <param name="flag">The flag value.</param>
        /// <returns>True if the enum value is valid, otherwise false.</returns>
        public bool ValidateEnumValue(int index, int flag) {
            Debug.LogFormat("index: {0}, flag: {1}",index,flag);
            if(!Value.GetType().IsEnum) return false;
            var casted = (int)Value;
            return casted == index || casted == flag;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}