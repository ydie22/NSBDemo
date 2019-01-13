namespace Billing
{
	using System.Threading.Tasks;
	using Messages;
	using NServiceBus;
	using NServiceBus.Logging;

	public class OrderPlacedHandler :
		IHandleMessages<OrderPlacedEvent>
	{
		static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();

		public Task Handle(OrderPlacedEvent message, IMessageHandlerContext context)
		{
			log.Info($"Billing has received OrderPlacedEvent, OrderId = {message.OrderId}. Sending invoice to customer");
			return Task.CompletedTask;
		}
	}
}