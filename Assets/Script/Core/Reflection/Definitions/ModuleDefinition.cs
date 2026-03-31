using System;

namespace Moonity.Core.Reflection
{
    public class ModuleDefinition
    {
        public string ModuleName { get; }
        public string ParentModuleName { get; }
        public string FullModuleName { get; }
        public string Description { get; }
        public Type Type { get; }

        public bool HasParentModule => !string.IsNullOrWhiteSpace(ParentModuleName);
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public ModuleDefinition(
            string moduleName,
            string parentModuleName,
            string fullModuleName,
            string description,
            Type type
        )
        {
            ModuleName = moduleName;
            ParentModuleName = parentModuleName;
            FullModuleName = fullModuleName;
            Description = description;
            Type = type;
            FullModuleName = null;
        }
    }
}
