using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
 
namespace TsunamiTattooSupply.Controllers.BackEnd
{
	 
	[Route("BackEnd/[controller]/[action]")]
	[Authorize]
	public class DashboardController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Dashboard/Index.cshtml");
		}

		[HttpGet]
		public IActionResult Index2()
		{
			return View("~/Views/BackEnd/Dashboard/Index2.cshtml");
		}

		[HttpGet]
		public IActionResult Index3()
		{
			return View("~/Views/BackEnd/Dashboard/Index3.cshtml");
		}


	}
}
