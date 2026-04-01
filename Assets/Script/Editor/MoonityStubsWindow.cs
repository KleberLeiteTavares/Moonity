#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using Moonity.Core.Modules;
using Moonity.Core.Reflection;
using Moonity.Core.Scripting;

namespace Moonity.Core.Unity.Editor
{
    public sealed class MoonityStubsWindow : EditorWindow
    {
        private const string OUTPUT_FOLDER = "Assets/Moonity/Stubs";
        private Assembly _assembly;


        [MenuItem("Moonity/Generate Lua Stubs")]
        public static void Open()
        {
            GetWindow<MoonityStubsWindow>("Moonity Stubs");
        }

        private void OnGUI()
        {
            GUILayout.Label("Moonity Stub Generator", EditorStyles.boldLabel);

            _assembly = typeof(MoonityModuleAttribute).Assembly;

            GUILayout.Label($"Assembly: {_assembly.GetName().Name}", EditorStyles.helpBox);

            GUILayout.Space(10);

            if (GUILayout.Button("Generate Stubs"))
                Generate();

            if (GUILayout.Button("Open Folder"))
            {
                if (Directory.Exists(OUTPUT_FOLDER))
                    EditorUtility.RevealInFinder(OUTPUT_FOLDER);
                else
                    Debug.LogWarning("Stubs folder does not exist yet.");
            }
        }

        private void Generate()
        {
            try
            {
                Assembly assembly = _assembly ?? Assembly.GetExecutingAssembly();

                var modules = ModulesFinder.FindAllFrom(assembly);

                Debug.Log($"[Moonity] Found {modules.Count} modules");

                Dictionary<string, string> files = StubsGenerator.GenerateStubs(modules);

                if (Directory.Exists(OUTPUT_FOLDER))
                    Directory.Delete(OUTPUT_FOLDER, true);

                Directory.CreateDirectory(OUTPUT_FOLDER);

                foreach (var kv in files)
                {
                    string fileName = kv.Key;
                    string content = kv.Value;

                    string path = Path.Combine(OUTPUT_FOLDER, fileName);

                    File.WriteAllText(path, content);
                }

                AssetDatabase.Refresh();

                Debug.Log($"[Moonity] Generated {files.Count} stub files at: {OUTPUT_FOLDER}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Moonity] Failed to generate stubs: {ex}");
            }
        }
    }
}
#endif