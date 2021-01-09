namespace IV.PCM.OSProcesses
{
	internal interface IProcessRunResult
	{
		int ExitCode { get; }
		string StandardOutput { get; }
		string StandardError { get; }
	}
}