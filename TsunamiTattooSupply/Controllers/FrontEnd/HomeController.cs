using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Models;
using System.Data.SqlClient;
using TsunamiTattooSupply.Data;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class HomeController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<HomeController> _logger;

		public HomeController(TsunamiDbContext dbContext,  ILogger<HomeController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);
			Global.BannerWebImagePath = fp.GetFilePath("BNNRWBIMG").Description;

			var vm = new PageViewModel
			{
				banners = GetBanners()
			};

			return View("~/Views/FrontEnd/Home/Index.cshtml",vm);
		}
		 
		public List<BannerDto> GetBanners()
		{ 
			try
			{

				 return  _dbContext.Banners
				.Where(b => b.DeletedDate == null &&
				            b.StatusID == "A")
				.Include(b => b.Status)
				.OrderBy(b => b.Rank)
				.Select(b => new BannerDto
				{
					ID = b.ID,
					Description = b.Description,
					Sentence = b.Sentence,
					Link = b.Link,
					ImagePath = Global.BannerWebImagePath,
					Image = b.Image,
					Rank = b.Rank,
					StatusID = b.Status.ID,
					Status = b.Status.Description,
					StatusColor = b.Status.Color

				}
				).ToList();
			}
			catch (Exception ex)
			{

				return new List<BannerDto>();
				_logger.LogError(ex, "Fetch Banners [ERROR]");

			}

		 
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
