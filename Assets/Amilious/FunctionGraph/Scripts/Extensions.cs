using System;
using System.Text;

namespace Amilious.FunctionGraph {
    public static class Extensions {

        public static string SplitCamelCase(this string value) {
            var sb = new StringBuilder();
            var chars = value.ToCharArray();
            for(var i = 0; i < chars.Length; i++) {
                var c = chars[i];
                if(char.IsUpper(c) && i != 0 && !char.IsUpper(chars[i - 1])) sb.Append(' ');
                if(i == 0) c = char.ToUpper(c);
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string SplitCamelCase(this Type type) {
            return type.Name.SplitCamelCase();
        }

    }
}