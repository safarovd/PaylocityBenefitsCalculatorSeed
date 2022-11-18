using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Relationship
{
    None, 
    Spouse,
    DomesticPartner, 
    Child
}
