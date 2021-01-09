using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
				.ConfigureLogging(ConfigureLogging)
				.ConfigureWebHostDefaults(whb => whb.UseStartup<Startup>())
				.ConfigureServices((_, services) => services.AddHostedService<Worker>())
				.UseWindowsService();
		}

		// TODO: Move part of logging configuration to Startup (aka web-api only)?
		private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
		{
			var configuringDateTimeUtc = DateTime.UtcNow;
			// TODO: Remove hardcoded path
			var logFileName = string.Format(
				@"c:\Program Files\potential-computing-machine\logs\pcm_{0}.log",
				configuringDateTimeUtc.ToString("s").Replace(":", "-"));
			
			loggingBuilder
				// TODO: Implement custom source for event log
				.AddEventLog(eventLogSettings => eventLogSettings.Filter = (_, logLevel) => logLevel >= LogLevel.Warning)
				.AddFile(
					logFileName,
					fileLoggerOptions =>
					{
						fileLoggerOptions.MaxRollingFiles = 5;
						fileLoggerOptions.Append = true;
					});
		}
	}
}