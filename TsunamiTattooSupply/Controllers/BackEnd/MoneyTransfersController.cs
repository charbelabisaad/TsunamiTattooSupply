using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class MoneyTransfersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/MoneyTransfers/Index.cshtml");
		}
	}
}
