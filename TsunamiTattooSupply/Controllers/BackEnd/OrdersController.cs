using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Orders/Index.cshtml");
		}
	}
}
