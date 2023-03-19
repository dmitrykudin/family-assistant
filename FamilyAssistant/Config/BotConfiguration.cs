namespace FamilyAssistant.Config;

public class BotConfiguration
{
    public bool UseWebHook { get; set; }

    public string BotToken { get; init; }

    public string HostAddress { get; init; }

    public string Route { get; init; }

    public string SecretToken { get; init; }
}
