namespace Billing
{
	using System;
	using System.Threading.Tasks;
	using NServiceBus;

	class Program
	{
		static async Task Main()
		{
			Console.Title = "Billing";

			var endpointConfiguration = new EndpointConfiguration("Billing");

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseConventionalRoutingTopology();
			transport.ConnectionString("host=localhost;username=guest;password=guest");
			endpointConfiguration.EnableInstallers();

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);

			Console.WriteLine("Press Enter to exit.");
			Console.ReadLine();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}