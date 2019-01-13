namespace BusUtilities
{
	using System;
	using System.Threading.Tasks;
	using NServiceBus.Logging;
	using NServiceBus.Pipeline;

	public class LoggingBehavior : Behavior<IIncomingLogicalMessageContext>
	{
		static readonly ILog log = LogManager.GetLogger<LoggingBehavior>();

		public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
		{
			log.Info($"Received message of type {context.Message.Instance.GetType()} with ID {context.MessageId}");
			await next();
		}
	}
}