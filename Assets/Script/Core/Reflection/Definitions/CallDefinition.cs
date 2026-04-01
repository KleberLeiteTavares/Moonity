using System.Collections.Generic;
using System.Reflection;

using Moonity.Core.Lua;

namespace Moonity.Core.Reflection
{
    public sealed class CallDefinition
    {
        public string CallName { get; }
        public string Description { get; }
        public string ReturnDescription { get; }
        public LuaType? ReturnType { get; }
        public IReadOnlyList<CallParameterDefinition> ParameterDefinitions { get; }
        public MethodInfo MethodInfo { get; }

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool HasReturnDescription => !string.IsNullOrWhiteSpace(ReturnDescription);
        public bool HasReturnType => ReturnType != null;

        public CallDefinition(
            string callName,
            string description,
            string returnDescription,
            LuaType? returnType,
            IReadOnlyList<CallParameterDefinition> parameterDefinitions,
            MethodInfo methodInfo
        )
        {
            CallName = callName;
            Description = description;
            ReturnDescription = returnDescription;
            ReturnType = returnType;
            MethodInfo = methodInfo;
            ParameterDefinitions = parameterDefinitions;
        }
    }
}
