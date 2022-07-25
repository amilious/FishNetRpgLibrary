using System;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a Type.
    /// </summary>
    public static class TypeExtensions {
        
        /// <summary>
        /// This method is used to split a types name using camel case.
        /// </summary>
        /// <param name="type">The type you want to get a name for.</param>
        /// <returns>The name of the type split based on camel case.</returns>
        public static string SplitCamelCase(this Type type) => type.Name.SplitCamelCase();
        
    }
    
}