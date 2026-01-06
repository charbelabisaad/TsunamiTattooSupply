using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Orders/Index.cshtml");
		}
	}
}
