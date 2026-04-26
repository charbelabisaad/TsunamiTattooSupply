using Microsoft.AspNetCore.Mvc;

namespace TsunamiTattooSupply.ViewComponents
{
	public class FooterViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}

}
