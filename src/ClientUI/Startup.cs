using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
	using ClientUI;
	using Messages;
	using NServiceBus;

	public class Startup
	{

		IEndpointInstance _endpointInstance;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
					// This lambda determines whether user consent for non-essential cookies is needed for a given request.
					options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			var endpointConfiguration = new EndpointConfiguration("ClientUI");

			var transport = endpointConfiguration.UseTransport<LearningTransport>();

			var routing = transport.Routing();
			routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

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
						 name: "default",
						 template: "{controller=Home}/{action=Index}/{id?}");
			});

			applicationLifetime.ApplicationStopped.Register(() => _endpointInstance?.Stop().GetAwaiter().GetResult());

			//RouteConfig.RegisterRoutes(RouteTable.Routes);

			//ControllerBuilder.Current.SetControllerFactory(new InjectEndpointInstanceIntoController(_endpointInstance));

		}
	}
}
