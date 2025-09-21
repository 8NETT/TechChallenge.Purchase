namespace TechChallenge.Purchases.Core.EventHub;

public interface IEventHubClient
{
    Task PublishAsync<T>(string eventName, T payload, CancellationToken ct = default) where T : class;
}