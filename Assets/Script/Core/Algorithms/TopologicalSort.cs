using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonity.Core.Algorithms
{
    public static class TopologicalSort
    {
        public static IReadOnlyList<T> Sort<T>(
            IReadOnlyList<T> values,
            Func<T, string> key,
            Func<T, string> parent,
            out Dictionary<string, T> map
        )
        {
            Dictionary<string, T> localMap = values.ToDictionary(m => key(m));
            map = localMap;

            List<T> sorted = new();
            HashSet<string> visited = new();
            HashSet<string> visiting = new();

            void Visit(string name)
            {
                if (visited.Contains(name))
                    return;

                if (visiting.Contains(name))
                    throw new InvalidOperationException($"A cycle was detected involving '{name}'.");

                visiting.Add(name);

                T value = localMap[name];
                string parentKey = parent(value);

                if (!string.IsNullOrEmpty(parentKey))
                {
                    if (!localMap.ContainsKey(parentKey))
                        throw new InvalidOperationException($"Parent module '{parentKey}' not found for '{name}'.");

                    Visit(parentKey);
                }

                visiting.Remove(name);
                visited.Add(name);
                sorted.Add(value);
            }

            foreach (var module in values)
                Visit(key(module));

            return sorted;
        }
    }
}
