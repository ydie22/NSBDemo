namespace ClientUI
{
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;

	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
		}
	}
}