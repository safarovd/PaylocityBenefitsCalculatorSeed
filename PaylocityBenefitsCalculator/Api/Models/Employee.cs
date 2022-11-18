using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Salary { get; set; }
        public string? DateOfBirth { get; set; }
        public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
        private bool _hasPartner = false;
        public bool HasPartner
        {
            get
            {
                return _hasPartner;
            }
            set
            {
                _hasPartner = value;
            }
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
}
