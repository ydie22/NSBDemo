namespace ClientUI
{
	using BusUtilities;
	using Messages;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using NServiceBus;

	public class Startup
	{
		IEndpointInstance _endpointInstance;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			// Configure NServiceBus
			var endpointConfiguration = new EndpointConfiguration("ClientUI");

			var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
			transport.UseConventionalRoutingTopology();
			transport.ConnectionString("host=localhost");
			// this routing config is only needed for this endpoint, as it is
			// the only one to send this command message
			transport.Routing().RouteToEndpoint(typeof(PlaceOrderCommand), "Sales");
			endpointConfiguration.EnableInstallers();
			endpointConfiguration.Pipeline.Register(typeof(LoggingBehavior), "Logs incoming messages");
			var conventions = endpointConfiguration.Conventions();
			conventions.DefiningCommandsAs(
				type => type.Name.EndsWith("Command"));
			conventions.DefiningEventsAs(
				type => type.Name.EndsWith("Event"));

			_endpointInstance = Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false).GetAwaiter().GetResult();

			services.AddSingleton(_endpointInstance);
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});

			applicationLifetime.ApplicationStopped.Register(() => _endpointInstance?.Stop().GetAwaiter().GetResult());
		}
	}
}