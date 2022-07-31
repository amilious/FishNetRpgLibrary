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

using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions {

        #region Fields /////////////////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Queue<StringBuilder> StringBuilders = new(MAX_STRING_BUILDERS);
        private const int MAX_STRING_BUILDERS = 20;
        public const string APPEND_C_FORMAT = "<color=#{0}>{1}</color>";
        public const string PADDING_FORMAT = "{0}{1} {2}";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

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

        /// <summary>
        /// This method is used to format a string with the given color.
        /// </summary>
        /// <param name="text">The text that you want to apply the color formatting to.</param>
        /// <param name="color">The color that you want to apply to the given text.</param>
        /// <returns>The string formatted in the given color.</returns>
        public static string SetColor(this string text, string color) {
            color = color.TrimStart(' ', '#');
            return string.Format(APPEND_C_FORMAT, color, text);
        }

        /// <summary>
        /// This method is used to format a string with the given color.
        /// </summary>
        /// <param name="text">The text that you want to apply the color formatting to.</param>
        /// <param name="color">The color that you want to apply to the given text.</param>
        /// <returns>The string formatted in the given color.</returns>
        public static string SetColor(this string text, Color color) {
            return string.Format(APPEND_C_FORMAT, color.HtmlRGBA(), text);
        }

        /// <summary>
        /// This method is used to pad a text with a character on both sides of a string.
        /// </summary>
        /// <param name="text">The text that you want to pad with a character.</param>
        /// <param name="character">The padding character.</param>
        /// <param name="length">The total length to make the text.</param>
        /// <param name="startLength">The start padding length.</param>
        /// <returns>The padded text.</returns>
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