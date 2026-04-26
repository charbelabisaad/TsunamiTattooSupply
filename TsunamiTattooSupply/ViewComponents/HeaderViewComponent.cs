using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.ViewComponents
{
	public class HeaderViewComponent :ViewComponent 
	{
		public IViewComponentResult Invoke()
		{ 
			return View();

		}
	}
}
