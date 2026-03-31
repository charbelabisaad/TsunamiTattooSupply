using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.ViewModels;

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
			var vm = new PageViewModel
			{
				categories = GetGategories(),
				groups = GetGroups()
			};

			return View("~/Views/BackEnd/Stock/Index.cshtml", vm);
		}

		public IActionResult ListGetStock([FromBody] ProductFilterDto filter)
		{
			try
			{
				var stocks = GetStock(filter);

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

		public List<StockDto> GetStock(ProductFilterDto filter)
		{
			try
			{
				// ============================
				// 🔥 BASE PRODUCT QUERY
				// ============================
				var productQuery = _dbContext.Products
					.Where(p => p.DeletedDate == null)
					.AsQueryable();

				// ============================
				// 🔥 CATEGORY FILTER
				// ============================
				if (filter.CategoryId.HasValue)
				{
					var subCategoryIds = _dbContext.SubCategories
						.Where(sc => sc.CategoryID == filter.CategoryId.Value)
						.Select(sc => sc.ID);

					productQuery = productQuery.Where(p =>
						_dbContext.ProductsSubCategories.Any(psc =>
							psc.ProductID == p.ID &&
							subCategoryIds.Contains(psc.SubCategoryID)
						)
					);
				}

				// ============================
				// 🔥 SUB CATEGORY FILTER
				// ============================
				if (filter.SubCategoryIds?.Any() == true)
				{
					productQuery = productQuery.Where(p =>
						_dbContext.ProductsSubCategories.Any(psc =>
							psc.ProductID == p.ID &&
							filter.SubCategoryIds.Contains(psc.SubCategoryID)
						)
					);
				}

				// ============================
				// 🔥 GROUP FILTER
				// ============================
				if (filter.GroupIds?.Any() == true)
				{
					productQuery = productQuery.Where(p =>
						filter.GroupIds.Contains(p.GroupID)
					);
				}

				// ============================
				// 🔥 MAIN QUERY (JOIN WITH STOCK)
				// ============================
				var stocks = (
					from s in _dbContext.Stocks

					join p in productQuery
						on s.ProductID equals p.ID

					join ps in _dbContext.ProductsSizes
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
						&& ps.DeletedDate == null
						&& ps.StatusID == "A"

					select new StockDto
					{
						ID = s.ID,
						ProductID = s.ProductID,
						ProductName = p.Name,

						GroupID = p.Group.ID,
						GroupName = p.Group.Name,

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
					}
				).ToList();

				return stocks;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetch Stock! [ERROR]");
				return null;
			}
		}

		public List<CategoryDto> GetGategories()
		{
			return _dbContext.Categories
				.Where(c => c.DeletedDate == null)
				.OrderBy(c => c.Rank)
				.Select(c => new CategoryDto
				{
					ID = c.ID,
					Description = c.Description
				}).ToList();

		}

		public List<GroupDto> GetGroups()
		{
			return _dbContext.Groups
				.Where(c => c.DeletedDate == null)
				.OrderBy(c => c.TypeID)
				.ThenBy(c => c.Rank)
				.Select(c => new GroupDto
				{
					ID = c.ID,
					Name = c.Name,
					TypeDescription = c.GroupType.Description
				}).ToList();
		}

		[HttpGet]
		public IActionResult GetSubCategoriesByCategory(int categoryId)
		{
			var subCategories = _dbContext.SubCategories
				.Where(sc =>
					sc.DeletedDate == null &&
					sc.StatusID == "A" &&
					sc.CategoryID == categoryId)
				.OrderBy(sc => sc.Rank)
				.Select(sc => new
				{
					id = sc.ID,
					name = sc.Description
				})
				.ToList();

			return Json(subCategories);
		}

		[HttpPost]
		public IActionResult SaveStock(int id, int quantity, string barcode)
		{
			try
			{
				var stock = _dbContext.Stocks
					.Include(x => x.Product)
					.FirstOrDefault(x => x.ID == id);

				if (stock == null)
					return Json(new { success = false, message = "Stock not found" });

				// =====================================================
				// 🔴 GLOBAL BARCODE CHECK
				// =====================================================
				if (!string.IsNullOrWhiteSpace(barcode))
				{
					var existingBarcode = _dbContext.Stocks
						.Include(x => x.Product)
						.Where(x => x.Barcode == barcode
									&& x.DeletedDate == null
									&& x.ID != id) // exclude same row
						.Select(x => new
						{
							x.ProductID,
							ProductName = x.Product.Name
						})
						.FirstOrDefault();

					if (existingBarcode != null)
					{
						return Json(new
						{
							success = false,
							message = $"Barcode '{barcode}' already exists in product: {existingBarcode.ProductName}"
						});
					}
				}

				// =====================================================
				// 🟢 UPDATE
				// =====================================================
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
