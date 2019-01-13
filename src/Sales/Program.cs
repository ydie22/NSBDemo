namespace Sales
{
	using System;
	using System.Threading.Tasks;
	using BusUtilities;

	internal class Program
	{
		static async Task Main()
		{
			var endpointName = "Sales";
			Console.Title = endpointName;

			var endpointInstance = await BusConfigurator.ConfigureAndStartEndpointInstance(endpointName);

			Console.WriteLine("Press Enter to exit.");
			Console.ReadLine();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}