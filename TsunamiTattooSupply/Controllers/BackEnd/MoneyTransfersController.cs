using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class MoneyTransfersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/MoneyTransfers/Index.cshtml");
		}
	}
}
