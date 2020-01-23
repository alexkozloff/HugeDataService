using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HugeDataService.Generator
{
    [JsonConverter(typeof(BagJsonConverter))]
    public class Bag: Dictionary<string, object>
    {
        public IDictionary<string, Bag> Nested
        {
            get =>
                TryGetValue(nameof(Nested), out var children)
                    ? (IDictionary<string, Bag>) children
                    : throw new InvalidOperationException($"No more children");
            set => this[nameof(Nested)] = value;
        }
    }
}