using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;

namespace TsunamiTattooSupply.Controllers.BackEnd
{

	[Authorize]
	public class StockController : Controller
	{
		private readonly TsunamiDbContext _dbContext;

		public ILogger<StockController> _logger;

		public StockController(TsunamiDbContext dbContext, ILogger<StockController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Stock/Index.cshtml");
		}

		public IActionResult ListGetStock()
		{
			try
			{
				var stocks = GetStock();

				return Json(new { data = stocks, success = true });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					data = new List<StockDto>(),
					success = false,
					message = "Unexpected error occured while loading stock!"

				});
			}

		}

		public List<StockDto> GetStock()
		{

			List<StockDto> stocks = new List<StockDto>();

			try
			{

				stocks = _dbContext.Stocks
					.Where(s => s.UseInStock == true
					&  s.DeletedDate == null)
					.Select(s => new StockDto
					{
						ID = s.ID,
						ProductID = s.ProductID,
						ProductName = s.Product.Name,
						GroupID = s.Product.Group.ID,
						GroupName = s.Product.Group.Name,
						ProductTypeID = s.ProductType.ID,
						ProductTypeDescription = s.ProductType.Description,
						ProductDetailID = s.ProductDetail.ID,
						ProductDetailDescription = s.ProductDetail.Description,
						SizeID = s.Size.ID,
						SizeDescription = s.Size.Description,
						ColorID = s.Color.ID,
						ColorName = s.Color.Name,
						ColorCode = s.Color.Code,
						Quantity = s.Quantity,
						Barcode = s.Barcode

					}).ToList();

			}
			catch (Exception ex) {

				stocks = null;

				_logger.LogError(ex, "Fetch Stock! [ERROR]");

			}
			return stocks;
		}

		[HttpPost]
		public IActionResult SaveStock(int id, int quantity, string barcode)
		{
			try
			{
				var stock = _dbContext.Stocks.FirstOrDefault(x => x.ID == id);
				if (stock == null)
					return Json(new { success = false, message = "Stock not found" });

				stock.Quantity = quantity;
				stock.Barcode = barcode;

				_dbContext.SaveChanges();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveStock error");
				return Json(new { success = false, message = ex.Message });
			}

		}
	}
}
