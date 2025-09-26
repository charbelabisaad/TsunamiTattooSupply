using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ServicesController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Services/Index.cshtml");
		}
	}
}
