using System.Text.Json.Serialization;

namespace UnitTests;

public class Token
{
    [JsonPropertyName("token")]
    public string TokenString { get; set; }
}