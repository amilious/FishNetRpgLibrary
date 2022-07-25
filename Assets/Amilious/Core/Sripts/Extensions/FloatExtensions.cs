namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Float class.
    /// </summary>
    public static class FloatExtensions {
        
        /// <summary>
        /// This method is used to check if a float value is an integer.
        /// </summary>
        /// <param name="value">The value you want to check.</param>
        /// <returns>True if the value is an integer, otherwise returns false.</returns>
        public static bool IsInteger(this float value) {
            var x = (int) value;
            return value -x == 0;
        }
        
    }
}