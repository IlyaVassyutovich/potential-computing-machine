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
		public async Task<JsonResult> Suspend()
		{
			await powerManagementService.Suspend();
			// TODO: Implement exception handling middleware
			return new JsonResult(new { Success = true });
		}
	}
}