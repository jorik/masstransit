using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace concurrency.Components;

public class TestState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = default!;

    [ConcurrencyCheck]
    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
}
