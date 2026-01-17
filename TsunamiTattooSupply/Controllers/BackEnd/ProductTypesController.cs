using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ProductTypesController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		ILogger<ProductTypesController> _logger;

		public ProductTypesController(TsunamiDbContext dbContext, ILogger<ProductTypesController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}	

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/ProductTypes/Index.cshtml");
		}

		public IActionResult ListGetProductTypes()
		{	
			List<ProductTypeDto> producttypes = new List<ProductTypeDto>();

			try
			{ 
				producttypes = GetProductTypes();
				return Json(new { data = producttypes, success = true });

			} 
			catch (Exception ex) {
				 
				return Json(new { data = producttypes, success = false, message = "An unexpected error while loading product types!"});
			}

		}

		public List<ProductTypeDto> GetProductTypes() {

			List<ProductTypeDto> producttypes = new List<ProductTypeDto>();

			try
			{
				producttypes = _dbContext.ProductTypes
					.Where( pt => pt.DeletedDate == null)
					.Select( pt =>  new ProductTypeDto
					{
						ID = pt.ID,
						Description = pt.Description,
						ShowFront = pt.ShowFront,
						StatusID = pt.Status.ID,
						StatusDescription = pt.Status.Description,
						StatusColor = pt.Status.Color
					}).ToList();

			}
			catch (Exception ex) {

				_logger.LogError(ex, "Listing Product [ERROR]!");

				producttypes = null;
			
			}
		
				return producttypes;

		}

		[HttpPost]
		public IActionResult SaveProductType(int ID, string Description, bool ShowFront, string StatusID)
		{
			try
			{
				// Normalize
				Description = Description?.Trim();
			 
				// Validation
				if ( string.IsNullOrEmpty(Description))
				{
					return Json(new { error = true, message = "Product Type code is required." });
				}
 
				// ================= EDIT =================
				if (ID != 0)
				{
					var existingProductType = _dbContext.ProductTypes.FirstOrDefault(pt => pt.ID == ID);

					if (existingProductType == null)
					{
						return Json(new { error = true, message = "Description not found." });
					}

					var duplicate = _dbContext.ProductTypes
						.Any(pt => pt.Description.ToLower() == Description.ToLower()
							   && pt.ID != ID
							   && pt.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Description already exists!" });
					}

					existingProductType.Description = Description;
					existingProductType.ShowFront = ShowFront;
					existingProductType.StatusID = StatusID;
					existingProductType.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingProductType.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

				}
				// ================= CREATE =================
				else
				{
					var duplicate = _dbContext.ProductTypes
						.Any(pt => pt.Description.ToLower() == Description.ToLower()
							   && pt.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Product Type name already exists!" });
					}

					var newProductType = new ProductType
					{
						Description = Description, 
						ShowFront = ShowFront,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.ProductTypes.Add(newProductType);
					_dbContext.SaveChanges();
				}

				return Json(new
				{
					success = true,
					data = GetProductTypes()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Save Product Type ERROR]");
				return Json(new
				{
					error = true,
					message = $"An unexpected error occurred while saving description {Description}!"
				});
			}
		}

		[HttpPost]
		public IActionResult DeleteProductType(int ID)
		{
			try
			{
				var producttype = _dbContext.ProductTypes
					.FirstOrDefault(c => c.ID == ID && c.DeletedDate == null);

				if (producttype == null)
				{
					return Json(new
					{
						error = true,
						message = "Product Type not found or already deleted."
					});
				}

				int userId = 0;
				int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

				producttype.DeletedUserID = userId;
				producttype.DeletedDate = DateTime.UtcNow;

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					data = GetProductTypes()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Delete Product Type ERROR]");
				return Json(new
				{
					error = true,
					message = "An unexpected error occurred while deleting the product type."
				});
			}
		}

	}
}
