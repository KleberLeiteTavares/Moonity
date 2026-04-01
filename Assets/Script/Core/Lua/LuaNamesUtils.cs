using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moonity.Core.Lua
{
    public static class LuaNamesUtils
    {
        private static readonly Regex _luaIdentifierRegex = new("^[a-zA-Z][a-zA-Z0-9]*$");

        private static readonly HashSet<string> _luaKeywords = new()
        {
            "and", "break", "do", "else", "elseif", "end",
            "false", "for", "function", "goto", "if", "in",
            "local", "nil", "not", "or", "repeat", "return",
            "then", "true", "until", "while"
        };

        public static bool IsValidLuaIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (!_luaIdentifierRegex.IsMatch(name))
                return false;

            if (_luaKeywords.Contains(name))
                return false;

            return true;
        }

        public static string FromCSharp(Type type)
        {
            if (type.IsEnum)
                return "number";

            if (type == typeof(int) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(long) ||
                type == typeof(short))
                return "number";

            if (type == typeof(bool))
                return "boolean";

            if (type == typeof(string) || type == typeof(char))
                return "string";

            if (!type.IsValueType)
                return "table";

            return "any"; // Fallback.
        }
    }
}
