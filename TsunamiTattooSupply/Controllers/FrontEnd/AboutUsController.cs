using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class AboutUsController : Controller
	{
		private readonly TsunamiDbContext _dbContext; 
		private readonly ILogger<AboutUsController> _logger;
		private readonly string _imagesroot;
		 
		public AboutUsController(TsunamiDbContext dbContext, ILogger<AboutUsController> logger, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			_imagesroot = config["StaticFiles:ImageRoot"];
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.BannerPageWebImagePath = fp.GetFilePath("BNNRPGEWBIMG").Description;
			Global.AboutUsImagePath = fp.GetFilePath("ABTUSIMG").Description;

			var vm = new PageViewModel
			{
				bannerabout = GetBannerPages("WEB", "ABT"),
				about = GetAbout("ABT"),
				clientsstatistics = GetStatitics("CLNT"),
				clients = GetClients(),
				brandsstatistics = GetStatitics("BRND"),
				brands = GetGroups("BRD"),
				stocksstatistics = GetStatitics("PRDCT"),
				stocks = GetStock(),
			};

			return View("~/Views/FrontEnd/AboutUs/Index.cshtml", vm);

		}

		public BannerPageDto GetBannerPages(string AppType, string PageLocationCode)
		{
			return _dbContext.BannersPages.Where(bp =>  bp.AppType == AppType
													&& 	bp.PageLocation.Code == PageLocationCode
													&& bp.StatusID == "A"
													&& bp.DeletedDate == null)
										  .Select ( bp =>  new BannerPageDto
										  {
											  ID = bp.ID,
											  Name = bp.Name,
											  ImagePath = Global.BannerPageWebImagePath,
											  Image = bp.Image
										  }
										).FirstOrDefault();
		}

		public AboutDto GetAbout(string ID)
		{
			return _dbContext.Abouts.Where(a => a.ID == ID)
									.Select(a => new AboutDto { 
										ID = a.ID,
										ShortText1 = a.ShortText1,
										ShortText2 = a.ShortText2,
										LongText = a.LongText,
										ImagePath = Global.AboutUsImagePath,
										Image1 = a.Image1,
										Image2 = a.Image2,
										}).FirstOrDefault();
			 
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

	}
}
