using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace IV.PCM.OSProcesses
{
	internal interface IProcessRunner
	{
		Task<IProcessRunResult> RunProcess(
			string executablePath,
			IEnumerable<string> arguments);
	}

	internal class ProcessRunner : IProcessRunner
	{
		private readonly ILogger<ProcessRunner> logger;

		public ProcessRunner(ILogger<ProcessRunner> logger)
		{
			this.logger = logger;
		}

		public async Task<IProcessRunResult> RunProcess(
			string executablePath,
			IEnumerable<string> arguments)
		{
			if (string.IsNullOrWhiteSpace(executablePath))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(executablePath));

			// TODO: Move to IFSManager?
			if (!File.Exists(executablePath))
				throw new ArgumentException($"Executable not found at \"{executablePath}\".", nameof(executablePath));

			var processStartInfo = new ProcessStartInfo
			{
				FileName = executablePath,
				ErrorDialog = false,
				CreateNoWindow = true,
				// TODO: LoadUserProfile
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};
			// TODO: Create extension AddAll
			foreach (var argument in arguments)
				processStartInfo.ArgumentList.Add(argument);

			logger.LogDebug("Starting process.");
			// TODO: Testable adapter for Sys.Diag.Process
			using var process = Process.Start(processStartInfo);
			if (process == null)
				throw new InvalidOperationException("Unable to start process."); // TODO: Dump PSI

			logger.LogDebug("Reading stdout.");
			// TODO: Replace with event-based streams reading
			var stdout = await process.StandardOutput.ReadToEndAsync();
			logger.LogDebug("Reading stderr.");
			var stderr = await process.StandardError.ReadToEndAsync();

			logger.LogDebug("Waiting for process to complete.");
			var cancellationToken = CancellationToken.None; // TODO: Implement this
			await process.WaitForExitAsync(cancellationToken);

			var exitCode = process.ExitCode;
			if (exitCode == 0)
			{
				logger.LogDebug("Process completed successfully.");
				return new ProcessRunResult(exitCode, stdout, stderr);
			}

			logger.LogWarning($"Process completed with exit code {exitCode}.");
			return new ProcessRunResult(exitCode, stdout, stderr);
		}


		private sealed record ProcessRunResult(
			int ExitCode,
			string StandardOutput,
			string StandardError) : IProcessRunResult;
	}
}