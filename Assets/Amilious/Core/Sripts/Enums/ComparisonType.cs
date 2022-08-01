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

using System;

namespace Amilious.Core {
    
    /// <summary>
    /// This enum is used to represent comparison types.
    /// </summary>
    [Serializable]
    public enum ComparisonType {
        
        Equal,
        ApproximatelyEqual,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
        
    }
    
    /// <summary>
    /// This class is used to add methods to the <see cref="ComparisonType"/> enum.
    /// </summary>
    public static class ComparisonTypeExtension{
        
        #region Static Readonly Fields /////////////////////////////////////////////////////////////////////////////////

        private static readonly Type FloatType = typeof(float);
        private static readonly Type DoubleType = typeof(double);
        private static readonly Type DecimalType = typeof(decimal);
        private static readonly Type SByteType = typeof(sbyte);
        private static readonly Type ByteType = typeof(byte);
        private static readonly Type UShortType = typeof(ushort);
        private static readonly Type ShortType = typeof(short);
        private static readonly Type IntType = typeof(int);
        private static readonly Type UIntType = typeof(uint);
        private static readonly Type LongType = typeof(long);
        private static readonly Type ULongType = typeof(ulong);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region ////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to compare two short values.
        /// </summary>
        /// <param name="type">The comparison type.</param>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The second value.</param>
        /// <param name="approximateDelta">A delta value use if the comparison type is approximate.</param>
        /// <returns>The result of the comparison.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an unhandled comparison type is used.</exception>
        public static bool Compare<T>(this ComparisonType type, T valueA, T valueB, T approximateDelta = default) 
            where T : IComparable<T>, IConvertible, IEquatable<T>, IFormattable {
            return type switch {
                ComparisonType.Equal => valueA.CompareTo(valueB)==0,
                ComparisonType.NotEqual => valueA.CompareTo(valueB)!=0,
                ComparisonType.LessThan => valueA.CompareTo(valueB)<0,
                ComparisonType.LessThanOrEqual => valueA.CompareTo(valueB)<=0,
                ComparisonType.GreaterThan => valueA.CompareTo(valueB)>0,
                ComparisonType.GreaterThanOrEqual => valueA.CompareTo(valueB)>=0,
                ComparisonType.ApproximatelyEqual => Approximate(valueA,valueB, approximateDelta),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        /// <summary>
        /// This method is used to check if two values are approximately equal.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The second value.</param>
        /// <param name="delta">The acceptable difference between the numbers.</param>
        /// <typeparam name="T">The type of number.</typeparam>
        /// <returns>True if the two values are approximately equal.</returns>
        private static bool Approximate<T>(T valueA, T valueB, T delta = default) 
            where T : IComparable<T>, IConvertible, IEquatable<T>, IFormattable {
            var rawValues = new [] { valueA, valueB, delta };
            var type = typeof(T);
            if(type == FloatType) return rawValues is float[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == DoubleType) return rawValues is double[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == DecimalType) return rawValues is decimal[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == SByteType) return rawValues is sbyte[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == ByteType) return rawValues is byte[] values && Math.Max(values[0],values[1]) -Math.Min(values[0],values[1]) < values[2];
            if(type == ShortType) return rawValues is short[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == UShortType) return rawValues is ushort[] values && Math.Max(values[0],values[1]) -Math.Min(values[0],values[1]) < values[2];
            if(type == IntType) return rawValues is int[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == UIntType) return rawValues is uint[] values && Math.Max(values[0],values[1]) -Math.Min(values[0],values[1]) < values[2];
            if(type == LongType) return rawValues is long[] values && Math.Abs(values[0] - values[1]) < values[2];
            if(type == ULongType) return rawValues is ulong[] values && Math.Max(values[0],values[1]) -Math.Min(values[0],values[1]) < values[2];
            return valueA.CompareTo(valueB) == 0;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}