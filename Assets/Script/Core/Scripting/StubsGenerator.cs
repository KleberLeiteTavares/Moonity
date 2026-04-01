using System.Collections.Generic;
using System.Linq;
using System.Text;

using Moonity.Core.Algorithms;
using Moonity.Core.Lua;
using Moonity.Core.Reflection;

namespace Moonity.Core.Scripting
{
    public static class StubsGenerator
    {
        public static Dictionary<string, string> GenerateStubs(IReadOnlyList<ModuleDefinition> modules)
        {
            IReadOnlyList<ModuleDefinition> sortedModules = TopologicalSort.Sort(
                modules,
                (m) => m.ModuleName,
                (m) => m.ParentModuleName,
                out _
            );

            Dictionary<string, string> files = new();

            foreach (ModuleDefinition module in sortedModules)
            {
                string fileName = $"{module.FullModuleName}.lua";
                files[fileName] = GenerateModuleStub(module);
            }

            files["Moonity.lua"] = GenerateRoot(sortedModules);

            return files;
        }

        private static string GenerateRoot(IReadOnlyList<ModuleDefinition> modules)
        {
            StringBuilder code = new();

            code.AppendLine("-- Auto-generated");
            code.AppendLine("local Moonity = {}\n");

            foreach (ModuleDefinition module in modules)
            {
                string requirePath = module.FullModuleName;

                code.AppendLine($"{requirePath} = require(\"{requirePath}\")");
            }

            code.AppendLine("\nreturn Moonity");

            return code.ToString();
        }

        private static string GenerateModuleStub(ModuleDefinition module)
        {
            StringBuilder code = new();

            code.AppendLine("-- Auto-generated");
            code.AppendLine($"---@class {module.FullModuleName}");

            code.AppendLine("local M = {}\n");

            try
            {
                AppendEnums(code, module);
                AppendCalls(code, module);
            }
            catch
            {
                // TODO: warning
            }

            code.AppendLine("return M");

            return code.ToString();
        }

        private static void AppendCalls(StringBuilder code, ModuleDefinition module)
        {
            foreach (CallDefinition call in module.Calls)
            {
                code.AppendLine($"---{call.Description}");

                foreach (CallParameterDefinition param in call.ParameterDefinitions)
                {
                    string type = param.HasLuaType
                        ? param.LuaType.ToString().ToLower()
                        : LuaNamesUtils.FromCSharp(param.Type);

                    code.AppendLine($"---@param {param.ParameterName} {type} {param.Description}");
                }

                if (call.HasReturnType)
                {
                    string type = call.ReturnType.Value.ToString().ToLower();
                    code.AppendLine($"---@return {type} {call.ReturnDescription}");
                }

                string args = string.Join(", ", call.ParameterDefinitions.Select(p => p.ParameterName));

                code.AppendLine($"function M.{call.CallName}({args}) end\n");
            }
        }

        private static void AppendEnums(StringBuilder code, ModuleDefinition module)
        {
            foreach (EnumDefinition en in module.Enums)
            {
                string fullName = $"{module.FullModuleName}.{en.EnumName}";

                code.AppendLine($"---@class {fullName}");
                code.AppendLine($"M.{en.EnumName} = {{");

                foreach (var kv in en.Values)
                {
                    code.AppendLine($"    {kv.Key} = {kv.Value},");
                }

                code.AppendLine("}\n");
            }
        }
    }
}