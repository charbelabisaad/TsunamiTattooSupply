using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Contact/Index.cshtml");
		}
	}
}
