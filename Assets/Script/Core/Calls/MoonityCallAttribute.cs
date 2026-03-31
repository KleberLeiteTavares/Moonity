using System;

using Moonity.Core.Lua;

namespace Moonity.Core.Calls
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class MoonityCallAttribute : Attribute
    {
        public string CallName { get; }
        public string Description { get; }
        public string ReturnDescription { get; }
        public LuaType? ReturnType { get; }

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool HasReturnDescription => !string.IsNullOrWhiteSpace(ReturnDescription);
        public bool HasReturnType => ReturnType != null;

        public MoonityCallAttribute(
            string callName,
            string description,
            string returnDescription,
            LuaType returnType
        )
        {
            CallName = callName;
            Description = description;
            ReturnDescription = returnDescription;
            ReturnType = returnType;
        }
    }
}
