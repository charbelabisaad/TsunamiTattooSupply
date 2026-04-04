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
		private readonly string _imagesRoot;

		public HomeController(TsunamiDbContext dbContext,  ILogger<HomeController> logger, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			_imagesRoot = config["StaticFiles:ImagesRoot"];
		}

		[HttpGet]
		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);
			Global.BannerWebImagePath = fp.GetFilePath("BNNRWBIMG").Description;
			Global.GroupImagePath = fp.GetFilePath("GRPIMG").Description;

			var vm = new PageViewModel
			{
				banners = GetBanners(),
				groups = GetGroups("BRND")
			};

			return View("~/Views/FrontEnd/Home/Index.cshtml",vm);
		}
		 
		public List<BannerDto> GetBanners()
		{ 
			try
			{

				return _dbContext.Banners
				.Where(b => b.StatusID == "A" && b.DeletedDate == null)
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
				})
				.ToList();
			}
			catch (Exception ex)
			{

				return new List<BannerDto>();
				_logger.LogError(ex, "Fetch Banners [ERROR]");

			}
			 
		}

		public List<GroupDto> GetGroups(string typeID)
		{
			try
			{
				return _dbContext.Groups
					.Where(g => g.TypeID == typeID &&
								g.StatusID == "A" && 
								g.DeletedDate == null)
					.OrderBy(g => g.Rank)
					.Select(g => new GroupDto
					{
						ID = g.ID,
						Name = g.Name,
						Summary = g.Summary,
						ShowHome = g.ShowHome,
						Rank = g.Rank,
						Image = g.Image,
						ImagePath = Global.GroupImagePath,

						TypeID = g.GroupType != null ? g.GroupType.ID : "",
						TypeDescription = g.GroupType != null ? g.GroupType.Description : "",

						StatusID = g.Status != null ? g.Status.ID : "",
						Status = g.Status != null ? g.Status.Description : "",
						StatusColor = g.Status != null ? g.Status.Color : ""
					})
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetch Groups [ERROR]");
				return new List<GroupDto>();
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
