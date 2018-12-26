using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlacedEvent>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlacedEvent message, IMessageHandlerContext context)
        {
            log.Info($"Shipping has received OrderPlacedEvent, OrderId = {message.OrderId}. Packaging and shipping to customer");
            return Task.CompletedTask;
        }
    }
}