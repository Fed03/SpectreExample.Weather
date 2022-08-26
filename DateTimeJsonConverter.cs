using System.Text.Json;
using System.Text.Json.Serialization;

namespace giuneco.wth;

internal class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();
        return DateTimeOffset.FromUnixTimeSeconds(value).LocalDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(new DateTimeOffset(value.ToUniversalTime()).ToUnixTimeSeconds());
    }
}