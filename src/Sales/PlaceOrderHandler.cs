namespace Sales
{
	using System;
	using System.Threading.Tasks;
	using Dapper;
	using Messages;
	using NServiceBus;
	using NServiceBus.Logging;

	public class PlaceOrderHandler :
		IHandleMessages<PlaceOrderCommand>
	{
		static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

		public async Task Handle(PlaceOrderCommand message, IMessageHandlerContext context)
		{
			log.Info($"Received PlaceOrderCommand, OrderId = {message.OrderId}");

			// This is normally where some business logic would occur

			#region ThrowTransientException

			// Uncomment to test throwing transient exceptions
			//if (DateTime.Now.Ticks % 5 == 0)
			//{
			//	throw new Exception("Oops");
			//}

			#endregion

			// Uncomment to test throwing fatal exceptions
			//throw new Exception("BOOM");

			var orderPlaced = new OrderPlacedEvent
			{
				OrderId = message.OrderId
			};

			log.Info($"Publishing OrderPlacedEvent, OrderId = {message.OrderId}");

			var tx = context.SynchronizedStorageSession.SqlPersistenceSession().Transaction;
			await tx.Connection.ExecuteAsync("INSERT INTO Orders (Id) VALUES(@Id)", new {Id = message.OrderId}, tx);

			var publishOptions = new PublishOptions();
			// it is difficult to simulate a failure after transaction commit
			// but before message dispatching. Therefore, we can force immediate dispatch
			// to avoid batching and dispatching after commit.
			// This is a sign that NSB is already rather robust without enabling the Outbox
			//publishOptions.RequireImmediateDispatch();
			await context.Publish(orderPlaced, publishOptions);

			//throw new Exception("Exploded");
		}
	}
}