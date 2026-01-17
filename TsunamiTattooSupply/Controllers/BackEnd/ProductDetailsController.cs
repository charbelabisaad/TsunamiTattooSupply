using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ProductDetailsController : Controller
	{
		private readonly TsunamiDbContext _dbContext;

		ILogger<ProductDetailsController> _logger;

		public ProductDetailsController (TsunamiDbContext dbContext, ILogger<ProductDetailsController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/ProductDetails/Index.cshtml");
		}

		public IActionResult ListGetProductDetails()
		{
			List<ProductDetailDto> ProductDetails = new List<ProductDetailDto>();

			try
			{
				ProductDetails = GetProductDetails();
				return Json(new { data = ProductDetails, success = true });

			}
			catch (Exception ex)
			{

				return Json(new { data = ProductDetails, success = false, message = "An unexpected error while loading Product Details!" });
			}

		}

		public List<ProductDetailDto> GetProductDetails()
		{

			List<ProductDetailDto> ProductDetails = new List<ProductDetailDto>();

			try
			{
				ProductDetails = _dbContext.ProductDetails
					.Where(pt => pt.DeletedDate == null)
					.Select(pt => new ProductDetailDto
					{
						ID = pt.ID,
						Description = pt.Description,
						ShowFront = pt.ShowFront,
						StatusID = pt.Status.ID,
						StatusDescription = pt.Status.Description,
						StatusColor = pt.Status.Color
					}).ToList();

			}
			catch (Exception ex)
			{

				_logger.LogError(ex, "Listing Product [ERROR]!");

				ProductDetails = null;

			}

			return ProductDetails;

		}

		[HttpPost]
		public IActionResult SaveProductDetail(int ID, string Description, bool ShowFront, string StatusID)
		{
			try
			{
				// Normalize
				Description = Description?.Trim();

				// Validation
				if (string.IsNullOrEmpty(Description))
				{
					return Json(new { error = true, message = "Product Detail code is required." });
				}

				// ================= EDIT =================
				if (ID != 0)
				{
					var existingProductDetail = _dbContext.ProductDetails.FirstOrDefault(pt => pt.ID == ID);

					if (existingProductDetail == null)
					{
						return Json(new { error = true, message = "Description not found." });
					}

					var duplicate = _dbContext.ProductDetails
						.Any(pt => pt.Description.ToLower() == Description.ToLower()
							   && pt.ID != ID
							   && pt.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Description already exists!" });
					}

					existingProductDetail.Description = Description;
					existingProductDetail.ShowFront = ShowFront;
					existingProductDetail.StatusID = StatusID;
					existingProductDetail.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingProductDetail.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

				}
				// ================= CREATE =================
				else
				{
					var duplicate = _dbContext.ProductDetails
						.Any(pt => pt.Description.ToLower() == Description.ToLower()
							   && pt.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Product Detail name already exists!" });
					}

					var newProductDetail = new ProductDetail
					{
						Description = Description,
						ShowFront = ShowFront,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.ProductDetails.Add(newProductDetail);
					_dbContext.SaveChanges();
				}

				return Json(new
				{
					success = true,
					data = GetProductDetails()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Save Product Detail ERROR]");
				return Json(new
				{
					error = true,
					message = $"An unexpected error occurred while saving description {Description}!"
				});
			}
		}

		[HttpPost]
		public IActionResult DeleteProductDetail(int ID)
		{
			try
			{
				var ProductDetail = _dbContext.ProductDetails
					.FirstOrDefault(c => c.ID == ID && c.DeletedDate == null);

				if (ProductDetail == null)
				{
					return Json(new
					{
						error = true,
						message = "Product Detail not found or already deleted."
					});
				}

				int userId = 0;
				int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

				ProductDetail.DeletedUserID = userId;
				ProductDetail.DeletedDate = DateTime.UtcNow;

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					data = GetProductDetails()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Delete Product Detail ERROR]");
				return Json(new
				{
					error = true,
					message = "An unexpected error occurred while deleting the Product Detail."
				});
			}
		}

	}
}
