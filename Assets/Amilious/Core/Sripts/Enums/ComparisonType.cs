using System;
using UnityEngine;

namespace Amilious.Core.Enums {
    
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
    
    public static class ComparisonTypeExtension{
    
        public static bool Compare(this ComparisonType type, int valueA, int valueB) {
            return type switch {
                ComparisonType.Equal => valueA==valueB,
                ComparisonType.NotEqual => valueA!=valueB,
                ComparisonType.LessThan => valueA<valueB,
                ComparisonType.LessThanOrEqual => valueA<=valueB,
                ComparisonType.GreaterThan => valueA>valueB,
                ComparisonType.GreaterThanOrEqual => valueA>=valueB,
                ComparisonType.ApproximatelyEqual => valueA==valueB,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static bool Compare(this ComparisonType type, float valueA, float valueB) {
            return type switch {
                ComparisonType.Equal => valueA==valueB,
                ComparisonType.NotEqual => valueA!=valueB,
                ComparisonType.LessThan => valueA<valueB,
                ComparisonType.LessThanOrEqual => valueA<=valueB,
                ComparisonType.GreaterThan => valueA>valueB,
                ComparisonType.GreaterThanOrEqual => valueA>=valueB,
                ComparisonType.ApproximatelyEqual => Mathf.Approximately(valueA,valueB),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static bool Compare(this ComparisonType type, double valueA, double valueB) {
            return type switch {
                ComparisonType.Equal => valueA==valueB,
                ComparisonType.NotEqual => valueA!=valueB,
                ComparisonType.LessThan => valueA<valueB,
                ComparisonType.LessThanOrEqual => valueA<=valueB,
                ComparisonType.GreaterThan => valueA>valueB,
                ComparisonType.GreaterThanOrEqual => valueA>=valueB,
                ComparisonType.ApproximatelyEqual => Math.Abs(valueA - valueB) < Mathf.Epsilon,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
    }
    
}