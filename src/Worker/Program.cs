using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace IV.PCM.Worker
{
	[UsedImplicitly]
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args)
				.Build()
				.Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				.CreateDefaultBuilder(args)
				// TODO: Implement custom source for event log
				.ConfigureLogging(loggingBuilder => loggingBuilder.AddEventLog())
				.ConfigureWebHostDefaults(whb => whb.UseStartup<Startup>())
				.ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); })
				.UseWindowsService();
		}
	}
}