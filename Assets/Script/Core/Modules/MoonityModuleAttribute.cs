using System;

namespace Moonity.Core.Modules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MoonityModuleAttribute : Attribute
    {
        public string ModuleName { get; }
        public string ParentModuleName { get; }
        public string Description { get; }

        public bool HasParentModule => !string.IsNullOrWhiteSpace(ParentModuleName);
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public MoonityModuleAttribute(
            string moduleName,
            string parentModuleName,
            string description
        )
        {
            ModuleName = moduleName;
            ParentModuleName = parentModuleName;
            Description = description;
        }
    }
}
