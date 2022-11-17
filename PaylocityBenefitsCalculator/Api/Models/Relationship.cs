using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Relationship
{
    //[EnumMember(Value = "")]
    None, 
    //[EnumMember(Value = "Spouse")]
    Spouse,
    //[EnumMember(Value = "DomesticPartner")]
    DomesticPartner, 
    //[EnumMember(Value = "Child")]
    Child
}
