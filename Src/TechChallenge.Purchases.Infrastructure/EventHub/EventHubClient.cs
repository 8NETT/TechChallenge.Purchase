using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using TechChallenge.Purchases.Core.EventHub;
using TechChallenge.Purchases.Core.Options;

namespace TechChallenge.Purchases.Infrastructure.EventHub;

public sealed class EventHubClient : IEventHubClient, IAsyncDisposable
{
    private readonly EventHubProducerClient _producer;
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public EventHubClient(IOptions<EventHubOptions> opt)
    {
        var o = opt.Value;
        _producer = new EventHubProducerClient(o.ConnectionString, o.HubName);
    }

    public async Task PublishAsync<T>(string eventName, T payload, CancellationToken ct = default) where T : class
    {
        var envelope = new
        {
            type = eventName,
            occurredAtUtc = DateTime.UtcNow,
            data = payload
        };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(envelope, _json);
        var evt = new EventData(bytes);
        evt.ContentType = "application/json";
        evt.Properties["type"] = eventName;

        using var batch = await _producer.CreateBatchAsync(ct);
        if (!batch.TryAdd(evt))
            throw new InvalidOperationException("Evento excede o tamanho do batch.");
        await _producer.SendAsync(batch, ct);
    }

    public async ValueTask DisposeAsync() => await _producer.DisposeAsync();
}