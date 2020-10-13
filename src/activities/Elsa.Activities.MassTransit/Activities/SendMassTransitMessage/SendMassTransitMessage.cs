using System;
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
        DisplayName = "Send MassTransit Message",
        Description = "Send a message via MassTransit."
    )]
    public class SendMassTransitMessage : MassTransitBusActivity
    {
        public SendMassTransitMessage(ConsumeContext consumeContext, IBus bus) : base(bus, consumeContext)
        {
        }

        [ActivityProperty(Hint = "An expression that evaluates to the message to send.")]
        public object? Message { get; set; }

        [ActivityProperty(Hint = "The address of a specific endpoint to send the message to.")]
        public Uri EndpointAddress { get; set; }

        protected override bool OnCanExecute(ActivityExecutionContext context)
        {
            return Message != null;
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(
            ActivityExecutionContext context,
            CancellationToken cancellationToken)
        {
            var message = Message;

            if (EndpointAddress != null)
            {
                var endpoint = await SendEndpointProvider.GetSendEndpoint(EndpointAddress);
                await endpoint.Send(message, cancellationToken);
            }
            else
            {
                await SendEndpointProvider.Send(message, cancellationToken);
            }

            return Done();
        }
    }
}