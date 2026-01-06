using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class PriceSynchronizationController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/PriceSynchronization/Index.cshtml");
		}
	}
}
