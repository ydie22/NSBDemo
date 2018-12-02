using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales
{
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrderCommand>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static Random random = new Random();

        public Task Handle(PlaceOrderCommand message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrderCommand, OrderId = {message.OrderId}");

            // This is normally where some business logic would occur

            #region ThrowTransientException
            // Uncomment to test throwing transient exceptions
            //if (random.Next(0, 5) == 0)
            //{
            //    throw new Exception("Oops");
            //}
            #endregion

            // Uncomment to test throwing fatal exceptions
            //throw new Exception("BOOM");

            var orderPlaced = new OrderPlacedEvent
            {
                OrderId = message.OrderId
            };
            
            log.Info($"Publishing OrderPlacedEvent, OrderId = {message.OrderId}");
            
            return context.Publish(orderPlaced);
        }
    }
}
