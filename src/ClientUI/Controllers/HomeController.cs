namespace ClientUI.Controllers
{
	using System;
	using System.Dynamic;
	using System.Threading;
	using System.Threading.Tasks;
	using Messages;
	using Microsoft.AspNetCore.Mvc;
	using NServiceBus;

	public class HomeController : Controller
	{
		static int messagesSent;
		readonly IEndpointInstance _endpointInstance;

		public HomeController(IEndpointInstance endpointInstance)
		{
			_endpointInstance = endpointInstance;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> PlaceOrder()
		{
			var orderId = Guid.NewGuid().ToString().Substring(0, 8);

			var command = new PlaceOrderCommand {OrderId = orderId};

			// Send the command
			await _endpointInstance.Send(command)
				.ConfigureAwait(false);

			dynamic model = new ExpandoObject();
			model.OrderId = orderId;
			model.MessagesSent = Interlocked.Increment(ref messagesSent);

			return View(model);
		}
	}
}