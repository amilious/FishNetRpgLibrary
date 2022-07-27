using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a string.
    /// </summary>
    public static class StringExtensions {

        private static readonly Queue<StringBuilder> StringBuilders = new(MAX_STRING_BUILDERS);
        private const int MAX_STRING_BUILDERS = 20;
        public const string APPEND_C_FORMAT = "<color=#{0}>{1}</color>";
        public const string PADDING_FORMAT = "{0}{1} {2}";

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

        public static string SetColor(this string text, string color) {
            color = color.TrimStart(' ', '#');
            return string.Format(APPEND_C_FORMAT, color, text);
        }

        public static string SetColor(this string text, Color color) {
            return string.Format(APPEND_C_FORMAT, color.HtmlRGBA(), text);
        }

        public static string PadText(this string text, char character, int length, int startLength = 0) {
            var builder = RentStringBuilder();
            try {
                if(startLength > 0) {
                    builder.Append(character, startLength);
                    builder.Append(' ');
                }
                builder.Append(text.ToUpperInvariant());
                if(builder.Length + 1 >= length) return builder.ToString();
                builder.Append(' ');
                builder.Append(character, length - builder.Length);
                return builder.ToString();
            }
            finally { ReturnStringBuilder(builder); }
        }
        
    }
}