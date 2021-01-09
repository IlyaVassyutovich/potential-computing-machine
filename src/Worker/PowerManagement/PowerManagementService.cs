using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using IV.PCM.OSProcesses;
using Microsoft.Extensions.Logging;

namespace IV.PCM.PowerManagement
{
	public interface IPowerManagementService
	{
		Task Suspend();
	}

	internal class PowerManagementService : IPowerManagementService
	{
		private readonly ILogger<PowerManagementService> logger;
		private readonly IProcessRunner processRunner;

		public PowerManagementService(
			ILogger<PowerManagementService> logger,
			IProcessRunner processRunner)
		{
			this.logger = logger;
			this.processRunner = processRunner;
		}

		public async Task Suspend()
		{
			logger.LogDebug("Running `psshutdown` to suspend.");
			var processRunResult = await processRunner.RunProcess(
				@"c:\Program Files\PSTools\psshutdown.exe",
				new[] {"-d", "-t", "5"});
			logger.LogDebug($"`psshutdown` completed with exit code {processRunResult.ExitCode}.");

			if (processRunResult.ExitCode == 0)
				return;

			var logMessage = new StringBuilder()
				.AppendLine("Process run failed.")
				.AppendLine("Standard output:")
				.AppendLine(processRunResult.StandardOutput)
				.AppendLine("- - - - - -")
				.AppendLine("Standard error:")
				.AppendLine(processRunResult.StandardError)
				.ToString();
			logger.LogError(logMessage);

			throw new InvalidOperationException($"Process run failed: {processRunResult.ExitCode}")
			{
				Data =
				{
					["StandardOutput"] = processRunResult.StandardOutput,
					["StandardError"] = processRunResult.StandardError,
				}
			};
		}
	}
}