using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.Errors;

namespace Route.OrderManagementSystem.APIs.Controllers
{
	[Route("errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Error(int code)
		{
			if (code == 401)
				return Unauthorized(new ApiResponse(401));
			else if (code == 404)
				return NotFound(new ApiResponse(404));
			else
				return StatusCode(code);
		}
	}
}
