using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ItemsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Items/Index.cshtml");
		}
	}
}
