using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class BestSellersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/BestSellers/Index.cshtml");
		}
	}
}
