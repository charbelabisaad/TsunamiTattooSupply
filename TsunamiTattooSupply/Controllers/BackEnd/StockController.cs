using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class StockController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Stock/Index.cshtml");
		}
	}
}
