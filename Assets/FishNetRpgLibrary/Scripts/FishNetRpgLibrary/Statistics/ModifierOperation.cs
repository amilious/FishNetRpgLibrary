using System;
using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This enum represents the operations that can be preformed by a stat modifier.
    /// </summary>
    [Serializable]
    public enum ModifierOperation {
        Additive = 0,
        Multiplicative = 1,
        Override = 2
    }
    
    /// <summary>
    /// This class is used to extend the ModifierOperation enum.
    /// </summary>
    public static class ModifierOperationExtension{

        /// <summary>
        /// This method is used to apply a modifier to a value.
        /// </summary>
        /// <param name="operation">The operation that you want to execute.</param>
        /// <param name="valueToModify">The value that you want to modify.</param>
        /// <param name="amount">The operation amount.</param>
        /// <returns>The modified value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a ModifierOperation is missing.</exception>
        public static int ApplyModifier(this ModifierOperation operation, int valueToModify, float amount) {
            return operation switch {
                ModifierOperation.Additive => Mathf.RoundToInt(valueToModify+amount),
                ModifierOperation.Multiplicative => Mathf.RoundToInt(valueToModify*amount),
                ModifierOperation.Override => Mathf.RoundToInt(amount),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };
        }
        
    }
}