namespace Billing
{
	using System;
	using System.Threading.Tasks;
	using NServiceBus;
	using Sales;

	class Program
	{
		static async Task Main()
		{
			var endpointName = "Billing";
			Console.Title = endpointName;

			var endpointInstance = await BusConfigurator.ConfigureAndStartEndpointInstance(endpointName);

			Console.WriteLine("Press Enter to exit.");
			Console.ReadLine();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}