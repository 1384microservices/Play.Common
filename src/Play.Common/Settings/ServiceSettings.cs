namespace Play.Common.Settings;

public class ServiceSettings
{
    public string FQName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Authority { get; init; } = string.Empty;
    public string MessageBroker { get; init; }
}