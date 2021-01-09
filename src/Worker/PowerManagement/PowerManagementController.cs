using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IV.PCM.PowerManagement
{
	[Route("power")]
	public class PowerManagementController : ControllerBase
	{
		private readonly IPowerManagementService powerManagementService;

		public PowerManagementController(IPowerManagementService powerManagementService)
		{
			this.powerManagementService = powerManagementService;
		}


		[HttpGet]
		[Route("suspend")]
		public async Task<IActionResult> Suspend()
		{
			await powerManagementService.Suspend();
			return Ok();
		}
	}
}