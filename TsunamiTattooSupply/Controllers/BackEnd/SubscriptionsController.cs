using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class SubscriptionsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Subscriptions/Index.cshtml");
		}
	}
}
