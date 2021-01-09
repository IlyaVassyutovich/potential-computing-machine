using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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

		public PowerManagementService(ILogger<PowerManagementService> logger)
		{
			this.logger = logger;
		}

		public async Task Suspend()
		{
			// TODO: Check executable location
			var processStartInfo = new ProcessStartInfo
			{
				FileName = @"c:\Program Files\PSTools\psshutdown.exe",
				Arguments = "-d -t 5",
				ErrorDialog = false,
				CreateNoWindow = true,
				// TODO: LoadUserProfile
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};

			logger.LogDebug("Starting `psshutdown`.");
			using var process = Process.Start(processStartInfo);
			if (process == null)
			{
				logger.LogError("Process did not start.");
				throw new InvalidOperationException("WTF?");
			}

			var stdout = await process.StandardOutput.ReadToEndAsync();
			var stderr = await process.StandardError.ReadToEndAsync();

			// TODO: Timeout
			logger.LogDebug("Waiting for `psshutdown` to exit.");
			await process.WaitForExitAsync();
			logger.LogDebug($"`psshutdown` exited with error code \"{process.ExitCode}\".");

			if (process.ExitCode != 0)
			{
				var message = new StringBuilder()
					.AppendLine($"Process run failed: {process.ExitCode}")
					.AppendLine("Standard output:")
					.AppendLine(stdout)
					.AppendLine("- - - - - -")
					.AppendLine("Standard error:")
					.AppendLine(stderr)
					.ToString();

				logger.LogError(message);
				// TODO: Message
				// TODO: Exception rendering with data
				throw new InvalidOperationException($"Process run failed: {process.ExitCode}")
				{
					Data =
					{
						["StandardOutput"] = stdout,
						["StandardError"] = stderr,
					}
				};
			}
		}
	}
}