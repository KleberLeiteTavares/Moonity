using System;
using System.Collections.Generic;

namespace Moonity.Core.Reflection
{
    public class ModuleDefinition
    {
        public string ModuleName { get; }
        public string ParentModuleName { get; }
        public string FullModuleName { get; }
        public string Description { get; }
        public Type Type { get; }

        public IReadOnlyList<CallDefinition> Calls { get; }
        public IReadOnlyList<EnumDefinition> Enums { get; }

        public bool HasParentModule => !string.IsNullOrWhiteSpace(ParentModuleName);
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public bool IsEmpty => (Calls.Count == 0) && (Enums.Count == 0);

        public ModuleDefinition(
            string moduleName,
            string parentModuleName,
            string fullModuleName,
            string description,
            IReadOnlyList<CallDefinition> calls,
            IReadOnlyList<EnumDefinition> enums,
            Type type
        )
        {
            ModuleName = moduleName;
            ParentModuleName = parentModuleName;
            FullModuleName = fullModuleName;
            Description = description;
            Calls = calls;
            Enums = enums;
            Type = type;
            FullModuleName = null;
        }
    }
}
