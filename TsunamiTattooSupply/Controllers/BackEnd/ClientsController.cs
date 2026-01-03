using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ClientsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Clients/Index.cshtml");
		}
	}
}
