namespace Sales
{
	using System.Threading.Tasks;
	using Dapper;
	using Messages;
	using NServiceBus;
	using NServiceBus.Logging;

	public class OrderShippedHandler : IHandleMessages<OrderShippedEvent>
	{
		const int SHIPPED = 1;
		static readonly ILog log = LogManager.GetLogger<OrderShippedHandler>();

		public Task Handle(OrderShippedEvent message, IMessageHandlerContext context)
		{
			log.Info($"Received OrderShippedEvent, OrderId = {message.OrderId}. Updating order status to SHIPPED");
			var tx = context.SynchronizedStorageSession.SqlPersistenceSession().Transaction;
			return tx.Connection.ExecuteAsync("UPDATE Orders SET Status=@Status WHERE Id=@Id", new {Id = message.OrderId, Status = SHIPPED}, tx);
		}
	}
}