using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class ClientsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Clients/Index.cshtml");
		}
	}
}
