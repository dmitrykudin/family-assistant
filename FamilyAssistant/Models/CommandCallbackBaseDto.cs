using System.Text.Json.Serialization;

namespace FamilyAssistant.Models;

public class CommandCallbackBaseDto
{
    [JsonPropertyName("C")]
    public string Command { get; set; }
}
