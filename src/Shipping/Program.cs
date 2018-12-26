using System;

namespace Shipping
{
	using System.Threading.Tasks;
	using NServiceBus;

	class Program
	{
		static async Task Main(string[] args)
		{
			Console.Title = "Shipping";

			var endpointConfiguration = new EndpointConfiguration("Shipping");

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseConventionalRoutingTopology();
			transport.ConnectionString("host=localhost");
			endpointConfiguration.EnableInstallers();
			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type => type.Name.EndsWith("Command"));
			conventions.DefiningEventsAs(
				type => type.Name.EndsWith("Event"));

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);

			await Console.Out.WriteLineAsync("Press Enter to exit.");
			await Console.In.ReadLineAsync();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}
