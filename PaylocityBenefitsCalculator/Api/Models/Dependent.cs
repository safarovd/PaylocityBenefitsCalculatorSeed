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
        public string DateOfBirth { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Relationship Relationship { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
