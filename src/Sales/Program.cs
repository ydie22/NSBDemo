namespace Sales
{
	using System;
	using System.Threading.Tasks;
	using NServiceBus;

	class Program
	{
		static async Task Main()
		{
			Console.Title = "Sales";

			var endpointConfiguration = new EndpointConfiguration("Sales");

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseConventionalRoutingTopology();
			transport.ConnectionString("host=localhost;username=guest;password=guest");
			endpointConfiguration.EnableInstallers();
			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type => type.Name.EndsWith("Command"));
			conventions.DefiningEventsAs(
				type => type.Name.EndsWith("Event"));

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);

			Console.WriteLine("Press Enter to exit.");
			Console.ReadLine();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}