using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Moonity.Core.Algorithms;
using Moonity.Core.Modules;

namespace Moonity.Core.Reflection
{
    public static class ModulesFinder
    {
        private static readonly Regex _luaIdentifierRegex = new("^[a-zA-Z][a-zA-Z0-9]*$");

        private static readonly HashSet<string> _luaKeywords = new()
        {
            "and", "break", "do", "else", "elseif", "end",
            "false", "for", "function", "goto", "if", "in",
            "local", "nil", "not", "or", "repeat", "return",
            "then", "true", "until", "while"
        };

        private class TempModuleDefinition
        {
            public string ModuleName { get; }
            public string ParentModuleName { get; }
            public string FullModuleName { get; set; }
            public string Description { get; }
            public Type Type { get; }

            public bool HasParentModule => !string.IsNullOrWhiteSpace(ParentModuleName);

            public TempModuleDefinition(
                string moduleName,
                string parentModuleName,
                string description,
                Type type
            )
            {
                ModuleName = moduleName;
                ParentModuleName = parentModuleName;
                Description = description;
                Type = type;
                FullModuleName = null;
            }
        }

        public static IReadOnlyList<ModuleDefinition> FindAllFrom(Assembly assembly)
        {
            IEnumerable<Type> moduleTypes = assembly.GetTypes().Where(t => IsValidModuleType(t));

            List<TempModuleDefinition> tempModuleDefinitions = GetTempModuleDefinitions(moduleTypes);

            IReadOnlyList<TempModuleDefinition> sortedModuleDefinitions = TopologicalSort.Sort(
                tempModuleDefinitions,
                (m) => m.ModuleName,
                (m) => m.ParentModuleName,
                out Dictionary<string, TempModuleDefinition> modulesMap
            );

            return GetModuleDefinitions(sortedModuleDefinitions, modulesMap);
        }

        private static bool IsValidModuleType(Type type)
        {
            if (type.IsAbstract)
                return false;

            if (type.GetCustomAttribute<MoonityModuleAttribute>() == null)
                return false;

            return true;
        }

        private static List<TempModuleDefinition> GetTempModuleDefinitions(IEnumerable<Type> moduleTypes)
        {
            List<TempModuleDefinition> tempModules = new();
            foreach (Type module in moduleTypes)
            {
                MoonityModuleAttribute moduleAttribute = module.GetCustomAttribute<MoonityModuleAttribute>();

                if (!ValidateModuleAttribute(moduleAttribute))
                    continue;

                TempModuleDefinition definition = new TempModuleDefinition(
                    moduleAttribute.ModuleName,
                    moduleAttribute.ParentModuleName,
                    moduleAttribute.Description,
                    module
                );

                tempModules.Add(definition);
            }

            return tempModules;
        }

        private static bool ValidateModuleAttribute(MoonityModuleAttribute moduleAttribute)
        {
            if (string.IsNullOrWhiteSpace(moduleAttribute.ModuleName))
                return false;

            if (!IsValidLuaIdentifier(moduleAttribute.ModuleName))
                return false;

            return true;
        }

        private static bool IsValidLuaIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (!_luaIdentifierRegex.IsMatch(name))
                return false;

            if (_luaKeywords.Contains(name))
                return false;

            return true;
        }

        private static List<ModuleDefinition> GetModuleDefinitions(
            IReadOnlyList<TempModuleDefinition> sortedDefinitions,
            Dictionary<string, TempModuleDefinition> modulesMap
        )
        {
            List<ModuleDefinition> moduleDefinitions = new(sortedDefinitions.Count);
            for (int i = 0; i < sortedDefinitions.Count; i++)
            {
                TempModuleDefinition tempDefinition = sortedDefinitions[i];

                ModuleDefinition moduleDefinition = new ModuleDefinition(
                    tempDefinition.ModuleName,
                    tempDefinition.ParentModuleName,
                    GetFullModuleName(tempDefinition, modulesMap),
                    tempDefinition.Description,
                    tempDefinition.Type
                );

                moduleDefinitions.Add(moduleDefinition);
            }

            return moduleDefinitions;
        }

        private static string GetFullModuleName(
            TempModuleDefinition definition,
            Dictionary<string, TempModuleDefinition> modulesMap
        )
        {
            if (definition.FullModuleName != null)
                return definition.FullModuleName;

            if (definition.HasParentModule)
            {
                TempModuleDefinition parent = modulesMap[definition.ParentModuleName];
                definition.FullModuleName = $"{GetFullModuleName(parent, modulesMap)}.{definition.ModuleName}";
            }
            else
            {
                definition.FullModuleName = $"Moonity.{definition.ModuleName}";
            }

            return definition.FullModuleName;
        }
    }
}
