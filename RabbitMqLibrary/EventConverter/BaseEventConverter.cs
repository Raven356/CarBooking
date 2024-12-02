using RabbitMqLibrary.Events;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RabbitMqLibrary.EventConverter
{
    public class BaseEventConverter : JsonConverter<BaseEvent>
    {
        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                // Extract the type discriminator
                if (root.TryGetProperty("Type", out var typeProperty))
                {
                    var type = typeProperty.GetString();
                    var targetType = type switch
                    {
                        nameof(OrderStartedEvent) => typeof(OrderStartedEvent),
                        nameof(TimedHistoryEvent) => typeof(TimedHistoryEvent),
                        nameof(EndOrderEvent) => typeof(EndOrderEvent),
                        nameof(GetOrderWithCarEvent) => typeof(GetOrderWithCarEvent),
                        nameof(GetTimedEventHistoryEvent) => typeof(GetTimedEventHistoryEvent),
                        nameof(MakeTimedEventNotHandleEvent) => typeof(MakeTimedEventNotHandleEvent),
                        _ => throw new JsonException($"Unknown event type: {type}")
                    };

                    // Deserialize into the correct type
                    return (BaseEvent)JsonSerializer.Deserialize(root.GetRawText(), targetType, options);
                }

                throw new JsonException("Missing Type property for BaseEvent deserialization.");
            }
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            // Add the type discriminator to the JSON
            var typeDiscriminator = value.GetType().Name;
            writer.WriteStartObject();
            writer.WriteString("Type", typeDiscriminator);

            // Serialize the actual object
            var json = JsonSerializer.Serialize(value, value.GetType(), options);
            using (var document = JsonDocument.Parse(json))
            {
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }
    }
}
