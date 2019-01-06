namespace Shipping
{
	using System.Threading.Tasks;
	using Messages;
	using NServiceBus;
	using NServiceBus.Logging;

	public class OrderPlacedHandler :
		IHandleMessages<OrderPlacedEvent>
	{
		static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

		public Task Handle(OrderPlacedEvent message, IMessageHandlerContext context)
		{
			log.Info($"Shipping has received OrderPlacedEvent, OrderId = {message.OrderId}. Packaging and shipping to customer");
			return context.Publish(new OrderShippedEvent
				{OrderId = message.OrderId});
		}
	}
}