using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moonity.Core.Calls;
using Moonity.Core.Lua;

namespace Moonity.Core.Reflection
{
    public static class CallsFinder
    {

        public static IReadOnlyList<CallDefinition> FindAllFrom(Type moduleType)
        {
            IEnumerable<MethodInfo> targets = moduleType.GetMethods().Where(m => IsValidMethod(m));

            List<CallDefinition> calls = new();
            foreach (MethodInfo method in targets)
            {
                MoonityCallAttribute callAttribute = method.GetCustomAttribute<MoonityCallAttribute>();

                if (!ValidateCall(method, callAttribute))
                    continue;

                if (!TryGetParameters(method, out List<CallParameterDefinition> parameters))
                    continue;

                CallDefinition callDefinition = new(
                    callAttribute.CallName,
                    callAttribute.Description,
                    callAttribute.ReturnDescription,
                    callAttribute.ReturnType,
                    parameters,
                    method
                );

                calls.Add(callDefinition);
            }

            return calls;
        }

        private static bool IsValidMethod(MethodInfo m)
        {
            if (m.IsAbstract)
                return false;

            if (m.GetCustomAttribute<MoonityCallAttribute>() == null)
                return false;

            return true;
        }

        private static bool ValidateCall(MethodInfo method, MoonityCallAttribute callAttribute)
        {
            // TODO: add warnings
            if (string.IsNullOrWhiteSpace(callAttribute.CallName))
                return false;

            if (!LuaNamesUtils.IsValidLuaIdentifier(callAttribute.CallName))
                return false;

            if (callAttribute.HasReturnType && !LuaTypesUtils.Matches(method.ReturnType, callAttribute.ReturnType))
                return false;

            if (!callAttribute.HasReturnType && !LuaTypesUtils.IsValidLuaType(method.ReturnType))
                return false;

            return true;
        }

        private static bool TryGetParameters(MethodInfo method, out List<CallParameterDefinition> parameters)
        {
            parameters = new List<CallParameterDefinition>();

            ParameterInfo[] methodParameters = method.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                ParameterInfo parameter = methodParameters[i];

                MoonityCallParameterAttribute parameterAttribute = parameter.GetCustomAttribute<MoonityCallParameterAttribute>();

                if (parameterAttribute != null && !ValidateParameterAttribute(parameter, parameterAttribute))
                    return false;

                CallParameterDefinition parameterDefinition = new(
                    parameterAttribute?.ParameterName ?? parameter.Name,
                    parameterAttribute?.Description ?? string.Empty,
                    parameterAttribute?.LuaType ?? null,
                    parameter.ParameterType
                );

                parameters.Add(parameterDefinition);
            }

            return true;
        }

        private static bool ValidateParameterAttribute(ParameterInfo parameter, MoonityCallParameterAttribute parameterAttribute)
        {
            // TODO: add warnings
            if (string.IsNullOrWhiteSpace(parameterAttribute.ParameterName))
                return false;

            if (parameterAttribute.HasLuaType && !LuaTypesUtils.Matches(parameter.ParameterType, parameterAttribute.LuaType))
                return false;

            if (!LuaTypesUtils.IsValidLuaType(parameter.ParameterType))
                return false;

            return true;
        }
    }
}
