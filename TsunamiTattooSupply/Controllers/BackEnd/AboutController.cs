using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/About/Index.cshtml");
		}
	}
}
