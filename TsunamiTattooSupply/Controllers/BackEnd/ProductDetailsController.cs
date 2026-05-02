using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize(AuthenticationSchemes = "AdminScheme")]
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
					.Where(pd => pd.DeletedDate == null)
					.Select(pd => new ProductDetailDto
					{
						ID = pd.ID,
						Description = pd.Description,
						Rank = pd.Rank,
						ShowFront = pd.ShowFront,
						StatusID = pd.Status.ID,
						StatusDescription = pd.Status.Description,
						StatusColor = pd.Status.Color
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

		[HttpPost]
		public IActionResult SaveRankProductDetails([FromBody] List<ProductDetailDto> productdetails)
		{
			try
			{
				if (productdetails == null || productdetails.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<ProductDetailDto>()
					});
				}

				// Collect IDs
				var ids = productdetails.Select(c => c.ID).ToList();

				// Load affected Sizes once
				var dbProductDetails = _dbContext.ProductDetails
											 .Where(c => ids.Contains(c.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbProductDetails.ToDictionary(c => c.ID);

				// Update only Rank
				foreach (var dto in productdetails)
				{
					if (dbDict.TryGetValue(dto.ID, out var size))
					{
						size.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)


				return Json(new
				{
					success = true,
					message = "Product Details rank saved successfully",
					data = GetProductDetails()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveRankProductDetails [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the product details rank.",
					data = new List<ProductDetailDto>()
				});
			}
		}

	}
}
