using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class BestSellersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/BestSellers/Index.cshtml");
		}
	}
}
