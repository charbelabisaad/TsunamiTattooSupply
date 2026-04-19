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
			Global.ProductSmallImagePath = fp.GetFilePath("PRDSMLIMG").Description;
			Global.BannerPageWebImagePath = fp.GetFilePath("BNNRPGEWBIMG").Description;
			Global.CategoryWebImagePath = fp.GetFilePath("CTGWBIMG").Description;
			Global.AboutUsImagePath = fp.GetFilePath("ABTUSIMG").Description;

			var vm = new PageViewModel
			{
				banners = GetBanners(),
				clientsstatistics = GetStatitics("CLNT"),
				clients = GetClients(),
				brandsstatistics = GetStatitics("BRND"),
				brands = GetGroups("BRD"),
				stocksstatistics = GetStatitics("PRDCT"),
				stocks = GetStock(), 
				groups = GetGroupsFront("BRD"),
				newarrivals = GetNewArrivals(),
				banneradvertising = GetBannerAdvertising("WEB", "HMAD"),
				about = GetAbout("ABT"),
				categories = GetGategories(),

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

		public Statistic GetStatitics(string Code)
		{
			return _dbContext.Statistics
				.Where(s => s.Code == Code.ToString())
				.Select(s => new Statistic
				{
					ID = s.ID,
					Name = s.Name,
					IsCalculated = s.IsCalculated,
					Number = s.Number,
				}).FirstOrDefault();
		}

		public List<ClientsDto> GetClients()
		{
			return _dbContext.Clients 
				.Select(c => new ClientsDto
				{
					ID = c.ID,
					Name = c.Name,
					Email = c.Email,
					PhoneNumber = (c.Country != null ? c.Country.Code + " " : "") + c.PhoneNumber
				}).ToList();
		}
		 
		public List<GroupDto> GetGroups(string TypeID)
		{
			List<GroupDto> brands = new List<GroupDto>();

			try
			{

				brands = _dbContext.Groups
				.Where(g => g.TypeID == TypeID && g.DeletedDate == null && g.StatusID == "A")
				.Include(g => g.Status)
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
					TypeID = g.GroupType.ID,
					TypeDescription = g.GroupType.Description,
					StatusID = g.Status.ID,
					Status = g.Status.Description,
					StatusColor = g.Status.Color

				}
				).ToList();
			}
			catch (Exception ex)
			{

				brands = new List<GroupDto>();
				_logger.LogError(ex, "Fetch Groups [ERROR]");

			}

			return brands;

		}

		public List<StockDto> GetStock()
		{
			try
			{
				var stocksQuery =
					from s in _dbContext.Stocks.AsNoTracking()

					join p in _dbContext.Products.AsNoTracking()
						on s.ProductID equals p.ID

					join ps in _dbContext.ProductsSizes.AsNoTracking()
						on new
						{
							s.ProductID,
							s.SizeID,
							s.ProductTypeID,
							s.ProductDetailID
						}
						equals new
						{
							ps.ProductID,
							ps.SizeID,
							ps.ProductTypeID,
							ps.ProductDetailID
						}

					where s.UseInStock == true
						&& s.DeletedDate == null
						&& p.DeletedDate == null
						&& ps.DeletedDate == null
						&& ps.StatusID == "A"

					select new StockDto
					{
						ID = s.ID,
						ProductID = s.ProductID,
						ProductName = p.Name,
						 
					};

				return stocksQuery.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetch Stock! [ERROR]");
				return new List<StockDto>();
			}
		}

		public List<GroupDto> GetGroupsFront(string typeID)
		{
			try
			{
				return _dbContext.Groups
					.Where(g => g.TypeID == typeID
							 && g.StatusID == "A"
							 && g.ShowHome == true
							 && g.DeletedDate == null)
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

		public List<ProductDto> GetNewArrivals()
		{
			var products = (
				from p in _dbContext.Products

				join pc in _dbContext.ProductsColors on p.ID equals pc.ProductID
				join pi in _dbContext.ProductsImages on p.ID equals pi.ProductID

				where p.NewArrival
					&& pc.IsCover
					&& pi.IsInitial
					&& p.DeletedDate == null
					&& pc.DeletedDate == null
					&& pi.DeletedDate == null
					&& (p.NewArrivalDateExpiryDate == null
						|| p.NewArrivalDateExpiryDate >= DateTime.UtcNow)

				select new ProductDto
				{
					ID = p.ID,
					Name = p.Name,
					GroupName = p.Group.Name,
					SmallImagePath = Global.ProductSmallImagePath,
					SmallImage = pi.SmallImage,

					// 🔥 MIN PRICE
					MinPrice = _dbContext.Prices
						.Where(pr => pr.ProductID == p.ID
							&& pr.DeletedDate == null
							&& pr.UseInPrice
							&& pr.AmountNet != 0)
						.Min(pr => (decimal?)pr.AmountNet),

					// 🔥 MAX PRICE
					MaxPrice = _dbContext.Prices
						.Where(pr => pr.ProductID == p.ID
							&& pr.DeletedDate == null
							&& pr.UseInPrice
							&& pr.AmountNet != 0)
						.Max(pr => (decimal?)pr.AmountNet),

					// 🔥 Currency (from lowest price)
					CurrencySymbol = _dbContext.Prices
						.Where(pr => pr.ProductID == p.ID
							&& pr.DeletedDate == null
							&& pr.UseInPrice
							&& pr.AmountNet != 0)
						.OrderBy(pr => pr.AmountNet)
						.Select(pr => pr.Currency.Symbol)
						.First()
				}
			)
			.GroupBy(x => x.ID)
			.Select(g => g.First())
			.Take(4)
			.ToList();

			return products;
		}

		public BannerPageDto GetBannerAdvertising(string AppType, string PageLocationCode)
		{
			return _dbContext.BannersPages.Where(bp => bp.AppType == AppType
													&& bp.PageLocation.Code == PageLocationCode
													&& bp.StatusID == "A"
													&& bp.DeletedDate == null
													&& bp.StartDate <= DateTime.UtcNow
													&& (bp.EndDate == null || bp.EndDate >= DateTime.UtcNow))
											.Select(bp => new BannerPageDto
											{
												ID = bp.ID,
												Name = bp.Name,
												ImagePath = Global.BannerPageWebImagePath,
												Image = bp.Image,
												Link = bp.Link

											}).First();
		}


		public AboutDto GetAbout(string ID)
		{
			return _dbContext.Abouts
			.Where(a => a.ID == ID)
			.Select(a => new AboutDto
			{
				ID = a.ID,
				ShortText1 = a.ShortText1,
				ShortText2 = a.ShortText2,
				LongText = a.LongText,
				ImagePath = Global.AboutUsImagePath,
				Image1 = a.Image1,
				Image2 = a.Image2

			}).FirstOrDefault();

		}

		public List<CategoryDto> GetGategories()
		{
			return _dbContext.Categories
				.Where(c => c.StatusID == "A" 
						 && c.DeletedDate == null)
				.OrderBy(c => c.Rank)
				.Select(c => new CategoryDto
				{
					ID = c.ID,
					Description = c.Description,
					WebImagePath = Global.CategoryWebImagePath,
					WebImage = c.WebImage,
					Rank = c.Rank
				}).ToList();

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
