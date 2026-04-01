using System;

using Moonity.Core.Lua;

namespace Moonity.Core.Calls
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MoonityCallParameterAttribute : Attribute
    {
        public string ParameterName { get; }
        public string Description { get; }
        public LuaType? LuaType { get; }

        public MoonityCallParameterAttribute(
            string parameterName = "",
            string description = "",
            LuaType? luaType = null
        )
        {
            ParameterName = parameterName;
            Description = description;
            LuaType = luaType;
        }
    }
}
