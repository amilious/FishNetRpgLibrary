using System;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a list
    /// </summary>
    public static class ListExtensions {

        /// <summary>
        /// This method is used to sort a list of paths/
        /// </summary>
        /// <param name="list">The list.</param>
        public static void SortByPath(this List<string> list) {
            list.Sort((a, b) => {
                var aLevels = a.Split('/');
                var bLevels = b.Split('/');
                for(var i = 0; i < aLevels.Length; i++) {
                    if(i >= bLevels.Length) return 1;
                    var value = string.Compare(aLevels[i], bLevels[i], StringComparison.Ordinal);
                    if(value == 0) continue;
                    if(aLevels.Length != bLevels.Length && (i == aLevels.Length - 1 || i == bLevels.Length - 1))
                        return aLevels.Length < bLevels.Length ? 1 : -1;
                    return value;
                }
                return 0;
            });
        }
        
    }
    
}