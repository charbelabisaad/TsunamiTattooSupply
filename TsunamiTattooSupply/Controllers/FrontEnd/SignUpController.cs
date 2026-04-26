using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class SignUpController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/FrontEnd/SignUp/index.cshtml");
		}
		 
	}
}
