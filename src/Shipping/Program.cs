namespace Shipping
{
	using System;
	using System.Threading.Tasks;
	using BusUtilities;

	internal class Program
	{
		static async Task Main(string[] args)
		{
			var endpointName = "Shipping";
			Console.Title = endpointName;

			var endpointInstance = await BusConfigurator.ConfigureAndStartEndpointInstance(endpointName);

			await Console.Out.WriteLineAsync("Press Enter to exit.");
			await Console.In.ReadLineAsync();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}