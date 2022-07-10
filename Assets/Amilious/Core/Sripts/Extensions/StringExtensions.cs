using System.Collections.Concurrent;
using System.Text;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a string.
    /// </summary>
    public static class StringExtensions {

        private static readonly ConcurrentQueue<StringBuilder> StringBuilders = new();
        private const int MAX_STRING_BUILDERS = 20;

        /// <summary>
        /// This method is used to rent out a string builder.
        /// </summary>
        /// <returns>A string builder.</returns>
        private static StringBuilder RentStringBuilder() =>
            StringBuilders.TryDequeue(out var builder) ? builder.Clear() : new StringBuilder();

        /// <summary>
        /// This method is used to return a string builder to the queue.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        private static void ReturnStringBuilder(StringBuilder builder) {
            if(StringBuilders.Count > MAX_STRING_BUILDERS) return;
            StringBuilders.Enqueue(builder.Clear());
        }
        
        /// <summary>
        /// This method is used to add spaces to a string based on capital letters.
        /// </summary>
        /// <param name="value">The value that you want to spit based on camel case.</param>
        /// <param name="spitNumbers">If true numbers will be spit as well.</param>
        /// <returns>The newly formatted string split using camel case.</returns>
        public static string SplitCamelCase(this string value, bool spitNumbers = false) {
            var sb = RentStringBuilder();
            try{
                var chars = value.ToCharArray();
                for(var i = 0; i < chars.Length; i++) {
                    var c = chars[i];
                    if(char.IsDigit(c) && i != 0 && !char.IsDigit(chars[i - 1]) && spitNumbers) sb.Append(' ');
                    else if(char.IsUpper(c) && i != 0 && !char.IsUpper(chars[i - 1])) sb.Append(' ');
                    if(i == 0) c = char.ToUpper(c);
                    sb.Append(c);
                }
                return sb.ToString();
            }finally{ ReturnStringBuilder(sb); }
        }
        
    }
}