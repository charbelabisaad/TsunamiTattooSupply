using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class RolesController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Roles/Index.cshtml");
		}
	}
}
