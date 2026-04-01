using System;

namespace Moonity.Core.Lua
{
    // TODO: accepts Moonsharp types here.
    public static class LuaTypesUtils
    {
        public static bool IsValidLuaType(Type type)
        {
            if (type == null)
                return false;

            if (type.IsEnum)
                return true;

            if (type == typeof(void))
                return true;

            if (!type.IsValueType)
                return true;

            if (type == typeof(bool))
                return true;

            if (IsNumber(type))
                return true;

            if (type == typeof(string))
                return true;

            return false;
        }

        public static bool Matches(Type assignedReturnType, LuaType? declaredType)
        {
            if (declaredType == null)
                return false;

            if (assignedReturnType == null)
                return declaredType == LuaType.Nil;

            Type type = Nullable.GetUnderlyingType(assignedReturnType) ?? assignedReturnType;

            switch (declaredType.Value)
            {
                case LuaType.Nil:
                    if (!type.IsValueType)
                        return true;
                    break;

                case LuaType.Boolean:
                    if (type == typeof(bool))
                        return true;
                    break;

                case LuaType.Number:
                    if (IsNumber(type) || type.IsEnum)
                        return true;
                    break;

                case LuaType.String:
                    if (type == typeof(string) || type == typeof(char))
                        return true;
                    break;

                case LuaType.Function:
                    if (typeof(Delegate).IsAssignableFrom(type))
                        return true;
                    break;

                case LuaType.Table:
                    if (IsTableLike(type))
                        return true;
                    break;
            }

            if (declaredType == LuaType.String)
            {
                // TODO: Add a warning here.
                if (type != typeof(void))
                    return true;
            }

            return false;
        }

        private static bool IsNumber(Type type)
        {
            return type == typeof(byte) ||
                   type == typeof(sbyte) ||
                   type == typeof(short) ||
                   type == typeof(ushort) ||
                   type == typeof(int) ||
                   type == typeof(uint) ||
                   type == typeof(long) ||
                   type == typeof(ulong) ||
                   type == typeof(float) ||
                   type == typeof(double) ||
                   type == typeof(decimal);
        }

        private static bool IsTableLike(Type type)
        {
            if (type.IsArray)
                return true;

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) &&
                type != typeof(string))
                return true;

            return false;
        }
    }
}
