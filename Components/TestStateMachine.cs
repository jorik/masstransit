using concurrency.Components.Messages;
using MassTransit;

namespace concurrency.Components;

public class TestStateMachine : MassTransitStateMachine<TestState>
{
    public TestStateMachine()
    {
        Event(() => CreateInstance, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => FinalizeInstance, x => x.CorrelateById(context => context.Message.CorrelationId));

        InstanceState(x => x.CurrentState);

        Initially(
            When(CreateInstance)
                .TransitionTo(Created));

        During(Created,
            When(FinalizeInstance)
                .Publish(context => new InstanceFinalized(context.Saga.CorrelationId))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public State Created { get; private set; }

    public Event<CreateInstance> CreateInstance { get; private set; }
    public Event<FinalizeInstance> FinalizeInstance { get; private set; }
}
