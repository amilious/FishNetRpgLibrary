using System;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Dictionary class.
    /// </summary>
    public static class DictionaryExtensions {
        
        /// <summary>
        /// This method will try to get a value from the dictionary using the provided key then cast
        /// the object as the <see cref="value"/> type.
        /// </summary>
        /// <param name="dictionary">The dictionary the value is in.</param>
        /// <param name="key">The key for the value.</param>
        /// <param name="value">The casted value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>Ture if the value for the given key exists and can be
        /// cast to the provided type, otherwise returns false.</returns>
        public static bool TryGetCastValue<T>(this IDictionary<string, object> dictionary, string key, out T value) {
            if(dictionary == null) {
                value = default(T);
                return false;
            }
            if(dictionary.TryGetValue(key, out var dicValue)) {
                if(dicValue is T value1){ 
                    value = value1;
                    return true;
                }
                try {
                    value = (T) Convert.ChangeType(dicValue, typeof(T));
                    return true;
                }catch(InvalidCastException) {}
            }
            value = default(T);
            return false;
        }
        
    }
}