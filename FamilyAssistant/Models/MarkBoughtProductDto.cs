using System.Text.Json.Serialization;

namespace FamilyAssistant.Models;

public class MarkBoughtProductDto : CommandCallbackBaseDto
{
    public long Id { get; set; }

    [JsonPropertyName("C")]
    public string Command { get; set; }
}
