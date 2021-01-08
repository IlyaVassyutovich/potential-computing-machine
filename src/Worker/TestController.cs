using Microsoft.AspNetCore.Mvc;

namespace IV.PCM.Worker
{
	[ApiController]
	[Route("test")]
	public class TestController : ControllerBase
	{
		[HttpGet]
		[Route("get")]
		public IActionResult Get()
		{
			return Ok(new {success = true});
		}
	}
}
