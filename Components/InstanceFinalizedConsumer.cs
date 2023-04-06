using concurrency.Components.Messages;
using MassTransit;

namespace concurrency.Components;

public class InstanceFinalizedConsumer : IConsumer<InstanceFinalized>
{
    private readonly ILogger<InstanceFinalizedConsumer> _logger;

    public InstanceFinalizedConsumer(ILogger<InstanceFinalizedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<InstanceFinalized> context)
    {
        _logger.LogInformation("Received InstanceFinalized: {CorrelationId}", context.Message.CorrelationId);
        return Task.CompletedTask;
    }
}
