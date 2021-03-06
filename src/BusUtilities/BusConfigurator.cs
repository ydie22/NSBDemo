﻿namespace BusUtilities
{
	using System;
	using System.Data.SqlClient;
	using System.Threading.Tasks;
	using NServiceBus;
	using NServiceBus.Persistence.Sql;

	public static class BusConfigurator
	{
		public static async Task<IEndpointInstance> ConfigureAndStartEndpointInstance(string endpointName)
		{
			var endpointConfiguration = new EndpointConfiguration(endpointName);

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseConventionalRoutingTopology();
			transport.ConnectionString("host=localhost;RequestedHeartbeat=600");

			var persist = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Outbox>();
			persist.SqlDialect<SqlDialect.MsSqlServer>();
			persist.ConnectionBuilder(() => new SqlConnection($"Server=localhost;Initial catalog={endpointName};User Id={endpointName};Password={endpointName}"));

			// comment out the next three lines to disable the outbox
			var outboxSettings = endpointConfiguration.EnableOutbox();
			outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
			outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));

			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type => type.Name.EndsWith("Command"));
			conventions.DefiningEventsAs(
				type => type.Name.EndsWith("Event"));

			endpointConfiguration.EnableInstallers();

			endpointConfiguration.Pipeline.Register(typeof(LoggingBehavior), "Logs incoming messages");

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);
			return endpointInstance;
		}
	}
}