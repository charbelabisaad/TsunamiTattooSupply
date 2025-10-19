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

		public CategoriesController(TsunamiDbContext dbContext , ILogger<CategoriesController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
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
						WebImage = c.WebImage,
						MobileImage = c.MobileImage,
						StatusID = c.StatusID,
						Status = c.Status.Description,
						StatusColor = c.Status.Color
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
        // Normalize description (trim and uppercase)
        string normalizedDescription = Description?.Trim().ToUpper();

        // ===== Duplicate Check =====
        bool isDuplicate = _dbContext.Categories
            .Any(c => c.Description.ToUpper() == normalizedDescription && c.ID != ID);

        if (isDuplicate)
        {
            return Json(new
            {
                exists = true,
                error = false,
                message = $"Category '{Description}' already exists!"
            });
        }

        // ===== Ensure directories exist =====
        if (!Directory.Exists(Global.CategoryWebImagePath))
            Directory.CreateDirectory(Global.CategoryWebImagePath);
        if (!Directory.Exists(Global.CategoryBannerImagePath))
            Directory.CreateDirectory(Global.CategoryBannerImagePath);
        if (!Directory.Exists(Global.CategoryADImagePath))
            Directory.CreateDirectory(Global.CategoryADImagePath);
        if (!Directory.Exists(Global.CategoryMobileImagePath))
            Directory.CreateDirectory(Global.CategoryMobileImagePath);

        // Helper function to save file with custom name
        string SaveFile(IFormFile? file, string folder, string prefix, int categoryId)
        {
            if (file == null || file.Length == 0)
                return null;

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string ext = Path.GetExtension(file.FileName);
            string fileName = $"{prefix}_{categoryId}_{timestamp}{ext}";
            string fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
                file.CopyTo(stream);

            // Return relative web path
            return Path.Combine(folder.Replace("wwwroot\\", "").Replace("\\", "/"), fileName);
        }

        Category category;

        if (ID != 0)
        {
            // ===== Update =====
            category = _dbContext.Categories.FirstOrDefault(c => c.ID == ID);
            if (category == null)
                return Json(new { error = true, message = "Category not found." });

            category.Description = Description;
            category.Details = Details;
            category.StatusID = StatusID;
            category.EditUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
            category.EditDate = DateTime.UtcNow;

            // Save files only if new ones were uploaded
            if (WebImage != null)
                category.WebImage = SaveFile(WebImage, Global.CategoryWebImagePath, "CTG", ID);

            if (BannerImage != null)
                category.BannerImage = SaveFile(BannerImage, Global.CategoryBannerImagePath, "CTG_BNNR", ID);

            if (AD_Image1 != null)
                category.AD_Image1 = SaveFile(AD_Image1, Global.CategoryADImagePath, "CTG_AD1", ID);

            if (AD_Image2 != null)
                category.AD_Image2 = SaveFile(AD_Image2, Global.CategoryADImagePath, "CTG_AD2", ID);

            if (AD_Image3 != null)
                category.AD_Image3 = SaveFile(AD_Image3, Global.CategoryADImagePath, "CTG_AD3", ID);

            if (MobileImage != null)
                category.MobileImage = SaveFile(MobileImage, Global.CategoryMobileImagePath, "CTG_MBL", ID);
             
            _dbContext.Categories.Update(category);
        }
        else
        {
            // ===== Insert =====
            category = new Category
            {
                Description = Description,
                Details = Details,
                StatusID = StatusID,   
                CreatedUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]),
                CreationDate = DateTime.UtcNow 
            };
 
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges(); // to generate ID
          
            // Save files with proper naming
            if (WebImage != null)
                category.WebImage = SaveFile(WebImage, Global.CategoryWebImagePath, "CTG", category.ID);

            if (BannerImage != null)
                category.BannerImage = SaveFile(BannerImage, Global.CategoryBannerImagePath, "CTG_BNNR", category.ID);

            if (AD_Image1 != null)
                category.AD_Image1 = SaveFile(AD_Image1, Global.CategoryADImagePath, "CTG_AD1", category.ID);

            if (AD_Image2 != null)
                category.AD_Image2 = SaveFile(AD_Image2, Global.CategoryADImagePath, "CTG_AD2", category.ID);

            if (AD_Image3 != null)
                category.AD_Image3 = SaveFile(AD_Image3, Global.CategoryADImagePath, "CTG_AD3", category.ID);

            if (MobileImage != null)
                category.MobileImage = SaveFile(MobileImage, Global.CategoryMobileImagePath, "CTG_MBL", category.ID);

            _dbContext.Categories.Update(category);
        }

        _dbContext.SaveChanges();

        return Json(new
        {
            error = false,
            message = ID == 0 ? "Category added successfully." : "Category updated successfully.",
            categories = GetCategories()
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "SaveCategory [ERROR]");
        return Json(new
        {
            error = true,
            message = $"An unexpected error occurred while saving category '{Description}'."
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

				// Optional: if you use a status field instead of IsDeleted
				// category.StatusID = "I"; // mark as Inactive

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
