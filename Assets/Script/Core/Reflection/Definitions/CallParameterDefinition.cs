using System;

using Moonity.Core.Lua;

namespace Moonity.Core.Reflection
{
    public sealed class CallParameterDefinition
    {
        public string ParameterName { get; }
        public string Description { get; }
        public LuaType? LuaType { get; }
        public Type Type { get; }

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool HasLuaType => LuaType != null;

        public CallParameterDefinition(
            string parameterName,
            string description,
            LuaType? luaType,
            Type type
        )
        {
            ParameterName = parameterName;
            Description = description;
            LuaType = luaType;
            Type = type;
        }
    }
}
