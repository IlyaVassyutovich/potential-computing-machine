using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IV.PCM.Worker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> logger;

		public Worker(ILogger<Worker> logger)
		{
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			logger.LogDebug($"Worker started at {DateTimeOffset.Now}.");

			while (!stoppingToken.IsCancellationRequested)
				await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

			logger.LogDebug($"Worker stopping at {DateTimeOffset.Now}.");
		}
	}
}