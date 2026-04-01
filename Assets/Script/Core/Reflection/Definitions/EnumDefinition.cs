using System;
using System.Collections.Generic;

namespace Moonity.Core.Enums
{
    public sealed class EnumDefinition
    {
        public string EnumName { get; }
        public Type Type { get; }
        public IReadOnlyDictionary<string, object> Values { get; }

        public EnumDefinition(
            string enumName,
            Type type,
            IReadOnlyDictionary<string, object> values
        )
        {
            EnumName = enumName;
            Type = type;
            Values = values;
        }
    }
}
