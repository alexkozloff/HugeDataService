using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HugeDataService.Generator
{
    public class BagJsonConverter : JsonConverter<Bag>
    {
        public override Bag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Bag bag, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var (key, value) in bag)
            {
                if (key == nameof(Bag.Nested))
                {
                    continue;
                }
                
                writer.WritePropertyName(key);
                JsonSerializer.Serialize(writer, value, options);
            }
            
            writer.WriteEndObject();
        }
    }
}