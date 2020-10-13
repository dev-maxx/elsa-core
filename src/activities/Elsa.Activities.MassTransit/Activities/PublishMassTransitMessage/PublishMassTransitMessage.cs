using System.Threading;
using System.Threading.Tasks;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using MassTransit;

// ReSharper disable once CheckNamespace
namespace Elsa.Activities.MassTransit
{
    [ActivityDefinition(
        Category = "MassTransit",
        DisplayName = "Publish MassTransit Message",
        Description = "Publish an event via MassTransit."
    )]
    public class PublishMassTransitMessage : MassTransitBusActivity
    {
        public PublishMassTransitMessage(IBus bus, ConsumeContext consumeContext) : base(bus, consumeContext)
        {
        }

        [ActivityProperty(Hint = "An expression that evaluates to the event to publish.")]
        public object? Message { get; set; }

        protected override bool OnCanExecute(ActivityExecutionContext context) => Message != null;

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            await PublishEndpoint.Publish(Message!, cancellationToken);

            return Done();
        }
    }
}