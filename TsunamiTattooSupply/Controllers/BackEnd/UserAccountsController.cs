using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class UserAccountsController : Controller
	{
		public IActionResult Index()
		{
			return View("~/Views/BackEnd/UserAccounts/Index.cshtml");
		}
	}
}
