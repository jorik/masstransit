using MassTransit;

namespace concurrency.Components;

public class TestStateMachineDefinition : SagaDefinition<TestState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<TestState> sagaConfigurator)
    {
        sagaConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromMilliseconds(150)));
    }
}