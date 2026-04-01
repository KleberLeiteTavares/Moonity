using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moonity.Core.Enums;
using Moonity.Core.Lua;

namespace Moonity.Core.Reflection
{
    public static class EnumsFinder
    {
        public static IReadOnlyList<EnumDefinition> FindAllFrom(Type moduleType)
        {
            Assembly assembly = moduleType.Assembly;

            IEnumerable<Type> enumTypes = assembly.GetTypes().Where(t => IsValidEnum(t));

            List<EnumDefinition> enums = new();

            foreach (Type enumType in enumTypes)
            {
                MoonityEnumAttribute attribute = enumType.GetCustomAttribute<MoonityEnumAttribute>();

                if (!ValidateEnum(enumType, attribute))
                    continue;

                Dictionary<string, object> values = GetValues(enumType);

                EnumDefinition definition = new(
                    attribute.EnumName,
                    enumType,
                    values
                );

                enums.Add(definition);
            }

            return enums;
        }

        private static bool IsValidEnum(Type type)
        {
            if (!type.IsEnum)
                return false;

            if (type.GetCustomAttribute<MoonityEnumAttribute>() == null)
                return false;

            return true;
        }

        private static bool ValidateEnum(Type type, MoonityEnumAttribute attribute)
        {
            // TODO: warnings
            if (string.IsNullOrWhiteSpace(attribute.EnumName))
                return false;

            if (!LuaNamesUtils.IsValidLuaIdentifier(attribute.EnumName))
                return false;

            return true;
        }

        private static Dictionary<string, object> GetValues(Type enumType)
        {
            Dictionary<string, object> values = new();

            Array rawValues = Enum.GetValues(enumType);
            foreach (object value in rawValues)
            {
                string name = Enum.GetName(enumType, value);

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                values[name] = Convert.ToDouble(value);
            }

            return values;
        }
    }
}