using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class BannersHomeMobileController : Controller
	{ 
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<BannersHomeWebController> _logger;
		//private readonly IWebHostEnvironment _environment;
		private readonly string _imagesRoot;

		public BannersHomeMobileController(
			TsunamiDbContext dbContext,
			ILogger<BannersHomeWebController> logger,
			IWebHostEnvironment environment,
			IConfiguration config)
		{
			//_environment = environment;
			_logger = logger;
			_dbContext = dbContext;
			_imagesRoot = config["StaticFiles:ImagesRoot"];

		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.BannerMobileImagePath = fp.GetFilePath("BNNRMBLIMG").Description;

			var vm = new PageViewModel
			{
				categories = GetCategories(),
				subcategories = GetSubCategories(),
				products = GetProducts(),
				groups = GetGroups()
			};

			return View("~/Views/BackEnd/BannersHomeMobile/Index.cshtml", vm);
		}
		 
		[HttpGet]
		public IActionResult ListGetBanners()
		{
			try
			{
				var banners = GetBanners();
				return Json(new { data = banners, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<BannerMobileDto>(),
					success = false,
					message = "An unexpected error occurred while loading banners"
				});

			}
		}

		public List<BannerMobileDto> GetBanners()
		{
			try
			{
				return _dbContext.BannersMobiles
					.Where(b => b.DeletedDate == null)
					.OrderBy(b => b.Rank)
					.Select(b => new BannerMobileDto
					{
						ID = b.ID,
						Description = b.Description,
						ImagePath = Global.BannerMobileImagePath,
						Image = b.Image,

						// ✅ Use FK directly (clean + safe)
						CategoryID = b.CategoryID,
						CategoryDescription = b.Category != null ? b.Category.Description : null,

						SubCategoryID = b.SubCategoryID,
						SubCategoryDescription = b.SubCategoryID != null ? b.SubCategory.Description : null,

						GroupID = b.GroupID,
						GroupName = b.GroupID != null ? b.Group.Name : null,

						ProductID = b.ProductID, 
						ProductName = b.ProductID != null ? b.Product.Name : null,

						ShopNow = Convert.ToBoolean(b.ShopNow),

						StatusID = b.Status.ID,
						Status = b.Status.Description,
						StatusColor = b.Status.Color
					})
					.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetch Banners [ERROR]");
				return new List<BannerMobileDto>();
			}
		}

		public List<CategoryDto> GetCategories()
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

		[HttpGet]
		public List<SubCategoryDto> GetSubCategories()
		{
			return _dbContext.SubCategories
				.Where(sc =>
					sc.DeletedDate == null &&
					sc.StatusID == "A")
				.OrderBy(sc => sc.Rank)
				.Select(sc => new SubCategoryDto
				{
					ID = sc.ID,
					Description = sc.Description
				})
				.ToList();
		}

		[HttpGet]
		public List<GroupDto> GetGroups()
		{
			return _dbContext.Groups
				.AsNoTracking()
				.Where(g =>
					g.DeletedDate == null)
				.OrderBy(g => g.Name)
				.Select(g => new GroupDto
				{
					ID = g.ID,
					Name = g.Name 
				})
				.ToList();
		}

		[HttpGet]
		public List<ProductDto> GetProducts()
		{
			return _dbContext.ProductsSubCategories
				.AsNoTracking()
				.Where(ps =>
					ps.DeletedDate == null &&
					ps.Product.DeletedDate == null)
				.OrderBy(ps => ps.Product.Name)
				.Select(ps => new ProductDto
				{
					ID = ps.Product.ID,
					Name = ps.Product.Name,
					GroupName = ps.Product.Group.Name
				})
				.ToList();
		}

		[HttpPost]
		public IActionResult SaveBanner(
			int ID,
			string Description,
			int? CategoryID,
			int? SubCategoryID,
			int? GroupID,
			int? ProductID,
			bool ShopNow,
			bool none,
			string BannerLink, // 👈 add this
			string StatusID,
			IFormFile? Image)
		{
			try
			{
				string normalizedDescription = Description?.Trim() ?? string.Empty;

				if (string.IsNullOrWhiteSpace(normalizedDescription))
				{
					return Json(new
					{
						success = false,
						message = "Description is required."
					});
				}

				if (BannerLink == "BannerCategoryRadio")
				{
					if (CategoryID <= 0)
						return Json(new { success = false, message = "Category is required." });

					SubCategoryID = null;
					GroupID = null;
					ProductID = null;

				}
				else if (BannerLink == "BannerSubCategoryRadio")
				{
					if (SubCategoryID <= 0)
						return Json(new { success = false, message = "Sub Category is required." });

					CategoryID = null;
					GroupID= null;
					ProductID = null;
				}
				else if (BannerLink == "BannerGroupRadio")
				{
					if (GroupID <= 0)
						return Json(new { success = false, message = "Group is required." });

					CategoryID = null;
					SubCategoryID = null;
					ProductID = null;
				}
				else if (BannerLink == "BannerProductRadio")
				{
					if (ProductID <= 0)
						return Json(new { success = false, message = "Product is required." });

					CategoryID = null;
					SubCategoryID = null;
					GroupID = null;
				}
				else if (BannerLink == "BannerShopNowRadio" || BannerLink == "BannerNoneRadio")
				{
				 
					CategoryID = null;
					SubCategoryID = null;
					GroupID = null;
					ProductID = null;

				}

				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				bool exists = _dbContext.BannersMobiles.Any(b =>
					b.Description.Trim() == normalizedDescription &&
					b.DeletedDate == null &&
					b.ID != ID);

				if (exists)
				{
					return Json(new
					{
						success = false,
						exists = true,
						message = $"'{Description}' already exists!"
					});
				}

				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrWhiteSpace(urlPath))
						throw new Exception("Banner image path is not configured.");

					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				string? SaveFile(IFormFile? file, string urlFolder, string prefix, int bannerId)
				{
					if (file == null || file.Length == 0) return null;

					long maxSizeBytes = 100 * 1024;
					if (file.Length > maxSizeBytes)
						throw new Exception("Image size must not exceed 100 KB.");

					var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
					var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

					if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
						throw new Exception("Only JPG, JPEG, PNG, and WEBP images are allowed.");

					var physicalFolder = PhysicalPath(Global.BannerMobileImagePath);
					Directory.CreateDirectory(physicalFolder);

					var name = $"{prefix}_{bannerId}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
					var fullPath = Path.Combine(physicalFolder, name);

					using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
					file.CopyTo(stream);

					return name;
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				BannerMobile banner;
				bool isNew = ID == 0;

				if (isNew && (Image == null || Image.Length == 0))
				{
					return Json(new
					{
						success = false,
						message = "Image is required."
					});
				}

				if (isNew)
				{
					banner = new BannerMobile
					{
						Description = normalizedDescription,
						CategoryID = CategoryID,
						SubCategoryID = SubCategoryID,
						GroupID = GroupID,
						ProductID = ProductID,
						ShopNow = ShopNow,
						StatusID = StatusID,
						Image = "",
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.BannersMobiles.Add(banner);
					_dbContext.SaveChanges();
				}
				else
				{
					banner = _dbContext.BannersMobiles.FirstOrDefault(b => b.ID == ID && b.DeletedDate == null);

					if (banner == null)
					{
						return Json(new
						{
							success = false,
							message = "Banner not found."
						});
					}

					banner.Description = normalizedDescription;
					banner.CategoryID = CategoryID;
					banner.SubCategoryID = SubCategoryID;
					banner.GroupID = GroupID;
					banner.ProductID = ProductID;
					banner.ShopNow = ShopNow;	
					banner.StatusID = StatusID;
					banner.EditUserID = userId;
					banner.EditDate = DateTime.UtcNow;
				}

				if (IsTrue("ClearImage") && !string.IsNullOrEmpty(banner.Image))
				{
					var oldPath = Path.Combine(PhysicalPath(Global.BannerMobileImagePath), banner.Image);

					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);

					banner.Image = "";
				}

				if (Image != null && Image.Length > 0)
				{
					if (!string.IsNullOrEmpty(banner.Image))
					{
						var oldPath = Path.Combine(PhysicalPath(Global.BannerMobileImagePath), banner.Image);

						if (System.IO.File.Exists(oldPath))
							System.IO.File.Delete(oldPath);
					}

					banner.Image = SaveFile(Image, Global.BannerMobileImagePath, "BNRMOB", banner.ID) ?? "";
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = isNew ? "Mobile banner added successfully." : "Mobile banner updated successfully.",
					banner = new
					{
						banner.ID,
						banner.Description,
						banner.CategoryID,
						banner.SubCategoryID,
						banner.GroupID,
						banner.ProductID,
						banner.ShopNow,
						banner.StatusID,
						banner.Image
					}
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"SaveBannerMobile failed for '{Description}'");

				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}


		[HttpPost]
		public IActionResult DeleteBanner(int ID)
		{
			try
			{
				var banner = _dbContext.BannersMobiles.FirstOrDefault(b => b.ID == ID && b.DeletedDate == null);

				if (banner == null)
				{
					return Json(new { success = false, message = "Banner not found!" });
				}

				// ===== Soft Delete Fields =====
				int? userId = null;
				if (HttpContext.Request.Cookies.ContainsKey("UserID"))
				{
					int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId);
					userId = parsedUserId;
				}

				banner.DeletedUserID = userId;
				banner.DeletedDate = DateTime.UtcNow;

				// ===== Move Image to /DELETED folder =====
				void MoveToDeleted(string? fileName, string folder)
				{
					try
					{
						if (string.IsNullOrWhiteSpace(fileName))
							return;

						var webRoot = _imagesRoot;
						var relativeFolder = folder.Trim().TrimStart('/', '\\');
						var sourcePath = Path.Combine(webRoot, relativeFolder, fileName);

						if (!System.IO.File.Exists(sourcePath))
							return;

						var deletedFolder = Path.Combine(webRoot, relativeFolder, "DELETED");
						if (!Directory.Exists(deletedFolder))
							Directory.CreateDirectory(deletedFolder);

						var destPath = Path.Combine(deletedFolder, fileName);

						System.IO.File.Move(sourcePath, destPath, true);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Failed to move Banner file '{fileName}' from {folder} to DELETED folder.");
					}
				}

				FilePathService fp = new FilePathService(_dbContext);

				string bannerImagePath = Global.BannerMobileImagePath ?? "";

				if (!string.IsNullOrWhiteSpace(bannerImagePath))
				{
					MoveToDeleted(banner.Image, bannerImagePath);
				}

				_dbContext.BannersMobiles.Update(banner);
				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = $"Banner '{banner.Description}' deleted successfully.",
					banners = GetBanners()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "DeleteBanner [ERROR]");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while deleting the banner."
				});
			}
		}

		[HttpPost]
		public IActionResult SaveRankBanners([FromBody] List<BannerMobileDto> banners)
		{
			try
			{
				if (banners == null || banners.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<BannerMobileDto>()
					});
				}

				// Collect IDs
				var ids = banners.Select(c => c.ID).ToList();

				// Load affected categories once
				var dbBanners = _dbContext.BannersMobiles
											 .Where(c => ids.Contains(c.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbBanners.ToDictionary(c => c.ID);

				// Update only Rank
				foreach (var dto in banners)
				{
					if (dbDict.TryGetValue(dto.ID, out var banner))
					{
						banner.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)


				return Json(new
				{
					success = true,
					message = "Banners rank saved successfully",
					data = GetBanners()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "saveRankBanners [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the banners rank.",
					data = new List<BannerMobileDto>()
				});
			}
		}

	}
}
