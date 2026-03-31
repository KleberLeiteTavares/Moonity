using System;

namespace Moonity.Core.Enums
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public sealed class MoonityEnumAttribute : Attribute
    {
        public string EnumName { get; }

        public MoonityEnumAttribute(string enumName)
        {
            EnumName = enumName;
        }
    }
}
