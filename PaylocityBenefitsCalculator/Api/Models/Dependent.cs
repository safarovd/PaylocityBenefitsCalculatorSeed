using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Dependent
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //[JsonConverter(typeof(DateTimeConverter))]
        public string DateOfBirth { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Relationship Relationship { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
