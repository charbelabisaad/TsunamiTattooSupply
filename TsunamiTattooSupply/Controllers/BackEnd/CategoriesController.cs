using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.Functions;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
 
namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class CategoriesController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<CategoriesController> _logger;
		//private readonly IWebHostEnvironment _env;
		private readonly string _imagesRoot;

		public CategoriesController(TsunamiDbContext dbContext , ILogger<CategoriesController> logger, IWebHostEnvironment env, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			//_env = env;
			_imagesRoot = config["StaticFiles:ImagesRoot"];
		}

		public IActionResult Index()
		{

			FilePathService fp = new FilePathService(_dbContext);

			Global.CategoryWebImagePath = fp.GetFilePath("CTGWBIMG").Description;
			Global.CategoryBannerImagePath = fp.GetFilePath("CTGBNNRIMG").Description;
			Global.CategoryADImagePath = fp.GetFilePath("CTGADIMG").Description;
			Global.CategoryMobileImagePath = fp.GetFilePath("CTGMBLIMG").Description;

			Global.SubCategoryWebImagePath = fp.GetFilePath("SBCTGWBIMG").Description;
			Global.SubCategoryBannerImagePath = fp.GetFilePath("SBCTGBNNRIMG").Description;
			Global.SubCategoryMobileImagePath = fp.GetFilePath("SBCTGMBLIMG").Description;

			return View("~/Views/BackEnd/Categories/Index.cshtml");
		}
		 
		public IActionResult ListGetCategories()
		{
			try
			{
				var categories = GetCategories();
				return Json( new { data = categories , success = true});
			}
			catch (Exception ex) { 
			 
				return Json (new {
					data = new List<Category>(),
					success = false, message = "An unexpected error occurred while loading categories"
				});

			}
			
		}

		public List<CategoryDto> GetCategories() { 
		

			List<CategoryDto> categories = new List<CategoryDto>();

			try
			{

				categories = _dbContext.Categories
					.Where(c => c.DeletedDate == null)
					.Include(c => c.Status) 
					.Select(c => new CategoryDto
					{
						ID = c.ID,
						Description = c.Description,
						Rank = c.Rank,
						WebImagePath = Global.CategoryWebImagePath,
						WebImage = c.WebImage,
						MobileImagePath = Global.CategoryMobileImagePath,
						MobileImage = c.MobileImage,
						BannerImagePath = Global.CategoryBannerImagePath,
						BannerImage = c.BannerImage,
						AD_ImagePath = Global.CategoryADImagePath,
						AD_Image1 = c.AD_Image1,
						AD_Image2 = c.AD_Image2,
						AD_Image3 = c.AD_Image3,
						AD_Details = c.Details,
						StatusID = c.StatusID,
						Status = c.Status.Description,
						StatusColor = c.Status.Color,
						SubCategoryCount = _dbContext.SubCategories.Count(sc => sc.CategoryID == c.ID 
																			 && sc.DeletedDate == null)
					}


					).ToList();

			}
			catch (Exception ex) { 
			
				categories = null;
				_logger.LogError(ex, "Fetch Categories [ERROR]");
			
			}
		 
			return categories;

		}

		[HttpPost]
		public IActionResult SaveCategory(
			int ID,
			string Description,
			string SpecsLabel,
			IFormFile? BannerImage,
			IFormFile? WebImage,
			IFormFile? AD_Image1,
			IFormFile? AD_Image2,
			IFormFile? AD_Image3,
			string? Details,
			IFormFile? MobileImage,
			string StatusID)
			{
			try
			{
				string normalizedDescription = Description?.Trim() ?? string.Empty;
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				// Duplicate check
				bool exists = _dbContext.Categories.Any(c =>
					c.Description.ToUpper() == normalizedDescription.ToUpper() &&
					c.DeletedDate == null &&
					c.ID != ID);

				if (exists)
				{
					return Json(new
					{
						success = false,
						error = false,
						exists = true,
						message = $"Category '{Description}' already exists!"
					});
				}

				// URL -> physical helper
				string PhysicalPath(string urlPath)
				{
					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				// Save file (returns filename)
				string? SaveFile(IFormFile? file, string urlFolder, string prefix, int categoryId)
				{
					if (file == null || file.Length == 0) return null;

					var physicalFolder = PhysicalPath(urlFolder);
					Directory.CreateDirectory(physicalFolder);

					var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
					var ext = Path.GetExtension(file.FileName);
					var fileName = $"{prefix}_{categoryId}_{timestamp}{ext}";
					var fullPath = Path.Combine(physicalFolder, fileName);

					using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
					file.CopyTo(stream);

					return fileName;
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				// Clear flag handler
				string? HandleClearFlag(string flag, string? field, string urlFolder)
				{
					if (IsTrue(flag))
					{
						if (!string.IsNullOrEmpty(field))
						{
							var fullPath = Path.Combine(PhysicalPath(urlFolder), field);
							if (System.IO.File.Exists(fullPath))
								System.IO.File.Delete(fullPath);
						}
						return null;
					}
					return field;
				}

				// Replace file (delete old using PHYSICAL path, then save new)
				string? ReplaceFile(IFormFile? newFile, string? oldFile, string urlFolder, string prefix, int categoryId)
				{
					if (newFile != null && newFile.Length > 0)
					{
						if (!string.IsNullOrEmpty(oldFile))
						{
							var oldPath = Path.Combine(PhysicalPath(urlFolder), oldFile);
							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}
						return SaveFile(newFile, urlFolder, prefix, categoryId);
					}
					return oldFile;
				}

				Category category;
				bool isNew = ID == 0;

				if (isNew)
				{
					category = new Category
					{
						Description = Description.Trim(),
						Details = Details,
						StatusID = StatusID,
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Categories.Add(category);
					_dbContext.SaveChanges(); // generates ID
				}
				else
				{
					category = _dbContext.Categories.FirstOrDefault(c => c.ID == ID);
					if (category == null)
						return Json(new { success = false, error = true, message = "Category not found." });

					category.Description = Description.Trim();
					category.Details = Details ?? category.Details; // ✅ keep old behavior
					category.StatusID = StatusID;
					category.EditUserID = userId;
					category.EditDate = DateTime.UtcNow;
				}

				// Clear flags
				category.WebImage = HandleClearFlag("ClearWebImage", category.WebImage, Global.CategoryWebImagePath);
				category.BannerImage = HandleClearFlag("ClearBannerImage", category.BannerImage, Global.CategoryBannerImagePath);
				category.MobileImage = HandleClearFlag("ClearMobileImage", category.MobileImage, Global.CategoryMobileImagePath);
				category.AD_Image1 = HandleClearFlag("ClearADImage1", category.AD_Image1, Global.CategoryADImagePath);
				category.AD_Image2 = HandleClearFlag("ClearADImage2", category.AD_Image2, Global.CategoryADImagePath);
				category.AD_Image3 = HandleClearFlag("ClearADImage3", category.AD_Image3, Global.CategoryADImagePath);

				// Replace files
				category.WebImage = ReplaceFile(WebImage, category.WebImage, Global.CategoryWebImagePath, "CTG", category.ID);
				category.BannerImage = ReplaceFile(BannerImage, category.BannerImage, Global.CategoryBannerImagePath, "CTG_BNNR", category.ID);
				category.MobileImage = ReplaceFile(MobileImage, category.MobileImage, Global.CategoryMobileImagePath, "CTG_MBL", category.ID);
				category.AD_Image1 = ReplaceFile(AD_Image1, category.AD_Image1, Global.CategoryADImagePath, "CTG_AD1", category.ID);
				category.AD_Image2 = ReplaceFile(AD_Image2, category.AD_Image2, Global.CategoryADImagePath, "CTG_AD2", category.ID);
				category.AD_Image3 = ReplaceFile(AD_Image3, category.AD_Image3, Global.CategoryADImagePath, "CTG_AD3", category.ID);

				_dbContext.Update(category);
				_dbContext.SaveChanges();

				// ✅ restore old response shape for frontend
				return Json(new
				{
					success = true,
					error = false,
					exists = false,
					message = isNew ? "Category added successfully." : "Category updated successfully.",
					category = new
					{
						category.ID,
						category.Description,
						category.StatusID,
						category.WebImage,
						category.BannerImage,
						category.AD_Image1,
						category.AD_Image2,
						category.AD_Image3,
						category.MobileImage
					}
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"SaveCategory failed for '{Description}'");
				return Json(new
				{
					success = false,
					error = true,
					exists = false,
					message = "An unexpected error occurred while saving category."
				});
			}
		}


		[HttpPost]
		public IActionResult SaveRankCategories([FromBody] List<CategoryDto> categories)
		{
			try
			{
				if (categories == null || categories.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<CategoryDto>()
					});
				}

				// Collect IDs
				var ids = categories.Select(c => c.ID).ToList();

				// Load affected categories once
				var dbCategories = _dbContext.Categories
											 .Where(c => ids.Contains(c.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbCategories.ToDictionary(c => c.ID);

				// Update only Rank
				foreach (var dto in categories)
				{
					if (dbDict.TryGetValue(dto.ID, out var category))
					{
						category.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)


				return Json(new
				{
					success = true,
					message = "Categories rank saved successfully",
					data = GetCategories()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "saveRankCategories [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the categories rank.",
					data = new List<CategoryDto>()
				});
			}
		}



		[HttpPost]
		public IActionResult DeleteCategory(int ID)
		{
			try
			{
				var category = _dbContext.Categories.FirstOrDefault(c => c.ID == ID);

				if (category == null)
				{
					return Json(new { success = false, message = "Category not found!" });
				}

				// ===== Soft Delete Fields =====
				int? userId = null;
				if (HttpContext.Request.Cookies.ContainsKey("UserID"))
				{
					int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId);
					userId = parsedUserId;
				}

				category.DeletedUserID = userId;
				category.DeletedDate = DateTime.UtcNow;

				// ===== Move associated images to /DELETED folders =====
				void MoveToDeleted(string? fileName, string folder)
				{
					try
					{
						if (string.IsNullOrWhiteSpace(fileName))
							return;

						var webRoot = _imagesRoot; // e.g. C:\...\wwwroot
						var relativeFolder = folder.Trim().TrimStart('/', '\\');
						var sourcePath = Path.Combine(webRoot, relativeFolder, fileName);

						if (!System.IO.File.Exists(sourcePath))
							return;

						// Create DELETED subfolder if not exists
						var deletedFolder = Path.Combine(webRoot, relativeFolder, "DELETED");
						if (!Directory.Exists(deletedFolder))
							Directory.CreateDirectory(deletedFolder);

						// Build target path inside DELETED folder
						var destPath = Path.Combine(deletedFolder, fileName);

						// Move file safely
						System.IO.File.Move(sourcePath, destPath, overwrite: true);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Failed to move file '{fileName}' from {folder} to DELETED folder.");
					}
				}

				// Move each image type (if exists)
				MoveToDeleted(category.WebImage, Global.CategoryWebImagePath);
				MoveToDeleted(category.BannerImage, Global.CategoryBannerImagePath);
				MoveToDeleted(category.MobileImage, Global.CategoryMobileImagePath);
				MoveToDeleted(category.AD_Image1, Global.CategoryADImagePath);
				MoveToDeleted(category.AD_Image2, Global.CategoryADImagePath);
				MoveToDeleted(category.AD_Image3, Global.CategoryADImagePath);

				// ===== Save DB Changes =====
				_dbContext.Categories.Update(category);
				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = $"Category '{category.Description}' deleted successfully.",
					categories = GetCategories()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "DeleteCategory [ERROR]");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while deleting the category."
				});
			}
		}

		public IActionResult ListGetSubCategories(int CategoryID)
		{
			try
			{
				var subcategories = GetSubCategories(CategoryID);
				return Json(new { data = subcategories, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<SubCategory>(),
					success = false,
					message = "An unexpected error occurred while loading subcategories"
				});

			}

		}

		public List<SubCategoryDto> GetSubCategories(int CategoryID)
		{
 
			List<SubCategoryDto> subcategories = new List<SubCategoryDto>();

			try
			{

				subcategories = _dbContext.SubCategories
					.Where(sc => sc.CategoryID == CategoryID && sc.DeletedDate == null)
					.Include(sc => sc.Status)
					.OrderBy(sc => sc.Rank)
					.Select(sc => new SubCategoryDto
					{
						ID = sc.ID,
						Description = sc.Description,
						SpecsLabel = sc.SpecsLabel,
						WebImagePath = Global.SubCategoryWebImagePath,
						WebImage = sc.WebImage,
						MobileImagePath = Global.SubCategoryMobileImagePath,
						MobileImage = sc.MobileImage,
						Rank = sc.Rank,
						BannerImagePath = Global.SubCategoryBannerImagePath,
						BannerImage = sc.BannerImage, 
						StatusID = sc.StatusID,
						Status = sc.Status.Description,
						StatusColor = sc.Status.Color 
					}


					).ToList();

			}
			catch (Exception ex)
			{

				subcategories = null;
				_logger.LogError(ex, "Fetch Sub Categories [ERROR]");

			}

			return subcategories;

		}


	[HttpPost]
	public IActionResult SaveSubCategory(
	int ID,
	int CategoryID,
	string Description,
	string SpecsLabel,
	IFormFile? BannerImage,
	IFormFile? WebImage,
	IFormFile? MobileImage,
	string StatusID)
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				string normalizedDescription = Description.Trim();

				// =====================================================
				// 🔹 DUPLICATE CHECK
				// =====================================================
				bool exists = _dbContext.SubCategories.Any(s =>
					s.Description.Trim() == normalizedDescription &&
					s.DeletedDate == null &&
					s.ID != ID &&
					s.CategoryID == CategoryID
				);

				if (exists)
				{
					return Json(new
					{
						success = false,
						error = false,
						exists = true,
						message = $"Sub Category '{Description}' already exists!"
					});
				}

				// =====================================================
				// 🔹 HELPERS
				// =====================================================
				string PhysicalPath(string urlPath)
				{
					if (string.IsNullOrWhiteSpace(urlPath))
						throw new Exception("SubCategory image path is not configured.");

					var relative = urlPath.Trim().TrimStart('/', '\\');
					return Path.Combine(_imagesRoot, relative);
				}

				bool IsTrue(string key)
					=> string.Equals(Request.Form[key], "true", StringComparison.OrdinalIgnoreCase);

				string? SaveFile(IFormFile? file, string urlFolder, string prefix, int subId)
				{
					if (file == null || file.Length == 0) return null;

					var physicalFolder = PhysicalPath(urlFolder);
					Directory.CreateDirectory(physicalFolder);

					var ext = Path.GetExtension(file.FileName);
					var fileName = $"{prefix}_{subId}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
					var fullPath = Path.Combine(physicalFolder, fileName);

					using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
					file.CopyTo(stream);

					return fileName;
				}

				string? HandleClear(string flag, string? field, string urlFolder)
				{
					if (IsTrue(flag))
					{
						if (!string.IsNullOrEmpty(field))
						{
							var fullPath = Path.Combine(PhysicalPath(urlFolder), field);
							if (System.IO.File.Exists(fullPath))
								System.IO.File.Delete(fullPath);
						}
						return null;
					}
					return field;
				}

				string? ReplaceFile(IFormFile? newFile, string? oldFile, string urlFolder, string prefix, int subId)
				{
					if (newFile != null && newFile.Length > 0)
					{
						if (!string.IsNullOrEmpty(oldFile))
						{
							var oldPath = Path.Combine(PhysicalPath(urlFolder), oldFile);
							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}

						return SaveFile(newFile, urlFolder, prefix, subId);
					}
					return oldFile;
				}

				// =====================================================
				// 🔹 INSERT / UPDATE
				// =====================================================
				SubCategory sub;
				bool isNew = ID == 0;

				if (isNew)
				{
					sub = new SubCategory
					{
						CategoryID = CategoryID,
						Description = normalizedDescription,
						SpecsLabel = SpecsLabel.Trim(),
						StatusID = StatusID,
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.SubCategories.Add(sub);
					_dbContext.SaveChanges(); // generate ID
				}
				else
				{
					sub = _dbContext.SubCategories.FirstOrDefault(s => s.ID == ID);
					if (sub == null)
					{
						return Json(new
						{
							success = false,
							error = true,
							message = "Sub Category not found."
						});
					}

					sub.Description = normalizedDescription;
					sub.SpecsLabel = SpecsLabel.Trim();
					sub.StatusID = StatusID;
					sub.EditUserID = userId;
					sub.EditDate = DateTime.UtcNow;
				}

				// =====================================================
				// 🔹 CLEAR FLAGS
				// =====================================================
				sub.WebImage = HandleClear("ClearSubWebImage", sub.WebImage, Global.SubCategoryWebImagePath);
				sub.BannerImage = HandleClear("ClearSubBannerImage", sub.BannerImage, Global.SubCategoryBannerImagePath);
				sub.MobileImage = HandleClear("ClearSubMobileImage", sub.MobileImage, Global.SubCategoryMobileImagePath);

				// =====================================================
				// 🔹 REPLACE IMAGES
				// =====================================================
				sub.WebImage = ReplaceFile(WebImage, sub.WebImage, Global.SubCategoryWebImagePath, "SCTG_WEB", sub.ID);
				sub.BannerImage = ReplaceFile(BannerImage, sub.BannerImage, Global.SubCategoryBannerImagePath, "SCTG_BNR", sub.ID);
				sub.MobileImage = ReplaceFile(MobileImage, sub.MobileImage, Global.SubCategoryMobileImagePath, "SCTG_MBL", sub.ID);

				_dbContext.SaveChanges();

				// =====================================================
				// 🔹 RESPONSE
				// =====================================================
				return Json(new
				{
					success = true,
					error = false,
					exists = false,
					message = isNew
						? "Sub Category added successfully."
						: "Sub Category updated successfully.",
					subCategory = new
					{
						sub.ID,
						sub.Description,
						sub.SpecsLabel,
						sub.StatusID,
						sub.WebImage,
						sub.BannerImage,
						sub.MobileImage
					}
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveSubCategory failed");

				return Json(new
				{
					success = false,
					error = true,
					exists = false,
					message = "An unexpected error occurred while saving sub category."
				});
			}
		}
		 
		[HttpPost]
		public IActionResult SaveRankSubCategories([FromBody] List<CategoryDto> subcategories, int CategoryID)
		{
			try
			{
				if (subcategories == null || subcategories.Count == 0)
				{
					return Json(new
					{
						success = false,
						message = "No ranking data received",
						data = new List<CategoryDto>()
					});
				}

				// Collect IDs
				var ids = subcategories.Select(sc => sc.ID).ToList();

				// Load affected categories once
				var dbSubCategories = _dbContext.SubCategories
											 .Where(sc => ids.Contains(sc.ID))
											 .ToList();

				// Fast lookup
				var dbDict = dbSubCategories.ToDictionary(sc => sc.ID);

				// Update only Rank
				foreach (var dto in subcategories)
				{
					if (dbDict.TryGetValue(dto.ID, out var subcategory))
					{
						subcategory.Rank = dto.Rank;
					}
				}

				_dbContext.SaveChanges();

				// 🔁 Return refreshed list (ordered by Rank)


				return Json(new
				{
					success = true,
					message = "Sub Categories rank saved successfully",
					data = GetSubCategories(CategoryID)
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SaveRankCategories [ERROR]");

				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while saving the sub categories rank.",
					data = new List<SubCategoryDto>()
				});
			}
		}

		[HttpPost]
		public IActionResult DeleteSubCategory(int ID)
		{
			try
			{
				var sub = _dbContext.SubCategories.FirstOrDefault(x => x.ID == ID);

				if (sub == null)
				{
					return Json(new { success = false, message = "Sub Category not found!" });
				}

				// ===== Soft Delete Fields =====
				int? userId = null;
				if (HttpContext.Request.Cookies.ContainsKey("UserID"))
				{
					int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int parsedUserId);
					userId = parsedUserId;
				}

				sub.DeletedUserID = userId;
				sub.DeletedDate = DateTime.UtcNow;

				// ===== Move Images to /DELETED Folders =====
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

						System.IO.File.Move(sourcePath, destPath, overwrite: true);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Failed to move file '{fileName}' from {folder} to DELETED folder.");
					}
				}

				// ===== Move subcategory images =====
				MoveToDeleted(sub.WebImage, Global.SubCategoryWebImagePath);
				MoveToDeleted(sub.MobileImage, Global.SubCategoryMobileImagePath);
				MoveToDeleted(sub.BannerImage, Global.SubCategoryBannerImagePath);

				// ===== Save Soft Delete =====
				_dbContext.SubCategories.Update(sub);
				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = $"Sub Category '{sub.Description}' deleted successfully."
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete Sub Category [ERROR]");
				return Json(new
				{
					success = false,
					message = "An unexpected error occurred while deleting the sub category."
				});
			}
		}
 
	}

}
