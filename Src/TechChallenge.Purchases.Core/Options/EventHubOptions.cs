namespace TechChallenge.Purchases.Core.Options;

public sealed class EventHubOptions
{
    public string ConnectionString { get; set; } = default!;
    public string HubName { get; set; } = default!;
    public string? PartitionKeyField { get; set; } = "UserId";
}