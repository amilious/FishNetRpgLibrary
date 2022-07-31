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
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using Amilious.Core.Attributes;
using UnityEngine;
using Amilious.Core.Enums;

namespace Amilious.Core {

    public interface IComparisonMethod {
        Type GetNumberType { get; }
    }
    
    /// <summary>
    /// This struct is used to represent a comparison.
    /// </summary>
    /// <typeparam name="T">The type of the comparison.</typeparam>
    [Serializable]
    public class ComparisonMethod<T> : IComparisonMethod where T : IComparable<T>, IConvertible, IEquatable<T>, IFormattable {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, DynamicLabel(""), Tooltip("The comparison type.")] 
        private ComparisonType compareType = ComparisonType.GreaterThanOrEqual;
        [ShowIf(nameof(compareType),ComparisonType.ApproximatelyEqual)]
        [SerializeField, Tooltip("The acceptable difference between two values for the approximate comparison type.")] 
        private T approximateDelta;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the type of number that the compare is for.
        /// </summary>
        public Type GetNumberType => typeof(T);

        /// <summary>
        /// This property is used to get the comparison type.
        /// </summary>
        public ComparisonType ComparisonType => compareType;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to compare two values.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The second value.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Compare(T valueA, T valueB) =>
            compareType.Compare(valueA, valueB, approximateDelta);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}