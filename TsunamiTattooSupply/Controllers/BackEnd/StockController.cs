using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class StockController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Stock/Index.cshtml");
		}
	}
}
