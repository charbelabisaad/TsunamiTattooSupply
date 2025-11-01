using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using TsunamiTattooSupply.Functions;
using System.IO;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class CategoriesController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<CategoriesController> _logger;
		private readonly IWebHostEnvironment _env;

		public CategoriesController(TsunamiDbContext dbContext , ILogger<CategoriesController> logger, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_logger = logger;
			_env = env;
		}

		public IActionResult Index()
		{

			srvcFilePath fp = new srvcFilePath(_dbContext);

			Global.CategoryWebImagePath = fp.GetFilePath("CTGWBIMG").Description;
			Global.CategoryBannerImagePath = fp.GetFilePath("CTGBNNRIMG").Description;
			Global.CategoryADImagePath = fp.GetFilePath("CTGADIMG").Description;
			Global.CategoryMobileImagePath = fp.GetFilePath("CTGMBLIMG").Description;

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
						SubCategoryCount = _dbContext.SubCategories.Count(sc => sc.CategoryID == c.ID)
					}


					).ToList();

			}
			catch (Exception ex) { 
			
				categories = null;
				_logger.LogError(ex, "Fetch Cagegories [ERROR]");
			
			}
		 
			return categories;

		}


		[HttpPost]
		public IActionResult SaveCategory(
	int ID,
	string Description,
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
				string normalizedDescription = Description?.Trim().ToUpper() ?? string.Empty;
				int userId = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);

				// 🔹 Duplicate check
				bool exists = _dbContext.Categories
					.Any(c => c.Description.ToUpper() == normalizedDescription && c.ID != ID);

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

				// 🔹 Ensure image directories exist
				string[] dirs =
				{
			Global.CategoryWebImagePath,
			Global.CategoryBannerImagePath,
			Global.CategoryADImagePath,
			Global.CategoryMobileImagePath
		};
				foreach (var dir in dirs)
					if (!Directory.Exists(dir))
						Directory.CreateDirectory(dir);

				// 🔹 File save helper
				string SaveFile(IFormFile? file, string urlFolder, string prefix, int categoryId)
				{
					if (file == null || file.Length == 0) return null;

					// urlFolder like "/Images/CATEGORY/WEB/" or "Images/CATEGORY/WEB"
					var webRoot = _env.WebRootPath; // e.g. ...\YourApp\wwwroot
					var relative = urlFolder.Trim().TrimStart('/', '\\'); // "Images/CATEGORY/WEB"
					var physicalFolder = Path.Combine(webRoot, relative); // ...\wwwroot\Images\CATEGORY\WEB

					Directory.CreateDirectory(physicalFolder); // safe even if exists

					var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
					var ext = Path.GetExtension(file.FileName);
					var fileName = $"{prefix}_{categoryId}_{timestamp}{ext}";
					var fullPath = Path.Combine(physicalFolder, fileName);

					// Optional: increase reliability on Windows
					using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						file.CopyTo(stream);
					}

					// return just the filename to store in DB
					return fileName;
				}

				// 🔹 Handle clear/delete logic
				string HandleClearFlag(string flag, string? field, string urlFolder)
				{
					if (string.Equals(Request.Form[flag], "true", StringComparison.OrdinalIgnoreCase))
					{
						if (!string.IsNullOrEmpty(field))
						{
							// Convert URL-like path (/Images/CATEGORY/WEB/) to a physical path under wwwroot
							var webRoot = _env.WebRootPath;
							var relative = urlFolder.Trim().TrimStart('/', '\\'); // "Images/CATEGORY/WEB"
							var physicalFolder = Path.Combine(webRoot, relative);

							var fullPath = Path.Combine(physicalFolder, field);

							if (System.IO.File.Exists(fullPath))
								System.IO.File.Delete(fullPath);
						}

						// ✅ Important: nullify field to update DB
						return null;
					}

					// return unchanged if not cleared
					return field;
				}


				// 🔹 Replace file logic — returns updated filename
				string ReplaceFile(IFormFile? newFile, string? oldFile, string folder, string prefix, int categoryId)
				{
					if (newFile != null && newFile.Length > 0)
					{
						// delete old file first
						if (!string.IsNullOrEmpty(oldFile))
						{
							string oldPath = Path.Combine(folder, oldFile);
							if (System.IO.File.Exists(oldPath))
								System.IO.File.Delete(oldPath);
						}
						return SaveFile(newFile, folder, prefix, categoryId);
					}
					return oldFile;
				}

				Category category;
				bool isNew = (ID == 0);

				if (isNew)
				{
					// ===== INSERT =====
					category = new Category
					{
						Description = Description,
						Details = Details,
						StatusID = StatusID,
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Categories.Add(category);
					_dbContext.SaveChanges(); // get ID first
				}
				else
				{
					// ===== UPDATE =====
					category = _dbContext.Categories.FirstOrDefault(c => c.ID == ID);
					if (category == null)
						return Json(new { success = false, error = true, message = "Category not found." });

					category.Description = Description;
					category.Details = Details ?? category.Details;
					category.StatusID = StatusID;
					category.EditUserID = userId;
					category.EditDate = DateTime.UtcNow;
				}

				// ✅ Handle clear flags (set null if user cleared image)
				category.WebImage = HandleClearFlag("ClearWebImage", category.WebImage, Global.CategoryWebImagePath);
				category.BannerImage = HandleClearFlag("ClearBannerImage", category.BannerImage, Global.CategoryBannerImagePath);
				category.MobileImage = HandleClearFlag("ClearMobileImage", category.MobileImage, Global.CategoryMobileImagePath);
				category.AD_Image1 = HandleClearFlag("ClearADImage1", category.AD_Image1, Global.CategoryADImagePath);
				category.AD_Image2 = HandleClearFlag("ClearADImage2", category.AD_Image2, Global.CategoryADImagePath);
				category.AD_Image3 = HandleClearFlag("ClearADImage3", category.AD_Image3, Global.CategoryADImagePath);

				// ✅ Replace or keep current files
				category.WebImage = ReplaceFile(WebImage, category.WebImage, Global.CategoryWebImagePath, "CTG", category.ID);
				category.BannerImage = ReplaceFile(BannerImage, category.BannerImage, Global.CategoryBannerImagePath, "CTG_BNNR", category.ID);
				category.MobileImage = ReplaceFile(MobileImage, category.MobileImage, Global.CategoryMobileImagePath, "CTG_MBL", category.ID);
				category.AD_Image1 = ReplaceFile(AD_Image1, category.AD_Image1, Global.CategoryADImagePath, "CTG_AD1", category.ID);
				category.AD_Image2 = ReplaceFile(AD_Image2, category.AD_Image2, Global.CategoryADImagePath, "CTG_AD2", category.ID);
				category.AD_Image3 = ReplaceFile(AD_Image3, category.AD_Image3, Global.CategoryADImagePath, "CTG_AD3", category.ID);

				// ✅ Save changes
				_dbContext.Update(category);
				_dbContext.SaveChanges();

				// ✅ Return success JSON
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
					int.TryParse(HttpContext.Request.Cookies["UserID"], out int parsedUserId);
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

						var webRoot = _env.WebRootPath; // e.g. C:\...\wwwroot
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


	}

}
