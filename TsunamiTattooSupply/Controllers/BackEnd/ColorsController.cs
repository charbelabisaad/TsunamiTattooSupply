using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class ColorsController : Controller
	{
		private readonly TsunamiDbContext _dbcontext;
		public ILogger<ColorsController> _logger;


		public ColorsController(TsunamiDbContext dbcontext, ILogger<ColorsController> logger)
		{
			_dbcontext = dbcontext;
			_logger = logger;
		}	

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Colors/Index.cshtml");
		}

		public IActionResult ListGetColors()
		{
			List<ColorDto> colors = new List<ColorDto>();

			try
			{
				colors = GetColors();

				return Json(new { Data = colors , sucess = true });

			}
			catch (Exception ex) {
				_logger.LogError(ex, "Fetching colors![ERROR].");

				return Json(new
				{
					data = new List<object>(),
					success = false,
					message = "An expected error occurred while loading colors!"
				});
			
			}
 
		}

		public List<ColorDto> GetColors() { 
		
			return (_dbcontext.Colors.Where(c => c.DeletedDate == null)
				    .OrderBy(c => c.Name)
					.Select(c => new ColorDto
					{
						ID = c.ID,
						Code = c.Code,
						Name = c.Name,
						IsCustom = c.IsCustom,

					})).ToList();
		
		}

		[HttpPost]
		public IActionResult SaveColors(int ID, string Code, string Name, bool IsCustom)
		{
			int ColorID;

			try
			{

				if(ID != 0)
				{
					var existingColor = _dbcontext.Colors.FirstOrDefault(c => c.ID == ID);

					var existingColorName  = _dbcontext.Colors.FirstOrDefault(c => c.Name.Trim().ToLower() == Name.Trim().ToLower() && c.ID != ID && c.DeletedDate == null);

					if (existingColorName != null) {

						return Json(new { exists = true, message = "Color name already exists!" });
					
					}

					if (existingColor != null) {
						
						existingColor.Code = Code;
						existingColor.Name = Name;
						existingColor.IsCustom = IsCustom;
						 
						_dbcontext.SaveChanges();

					}

				}
				else
				{
					var existingColorName = _dbcontext.Colors.FirstOrDefault(c => c.Name.Trim().ToLower() == Name.Trim().ToLower() && c.DeletedDate == null);

					if (existingColorName != null)
					{

						return Json(new { exists = true, message = "Color name already exists!" });

					}

					var newColor = new Color
					{
						Code = Code,
						Name = Name,
						IsCustom = IsCustom
					};

					_dbcontext.Colors.Add(newColor);
					_dbcontext.SaveChanges();

					ColorID = newColor.ID;	

				}

				return Json ( new { exists = false, data = GetColors() });
				 
			}
			catch (Exception ex)
			{
				_logger.LogError($"[Save Color ERROR]!\n\n{ex.Message}");
				return Json(new
				{
					exists = false,
					error = true,
					message = $"An expected error occured while saving color {Name}!"
				});
			}

		}


		public IActionResult DeleteColor(int ID)
		{
			var existingColors = _dbcontext.Colors.FirstOrDefault(c => c.ID == ID && c.DeletedDate == null);

			try
			{
				

				if (existingColors != null) {

					existingColors.DeletedUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
					existingColors.DeletedDate = DateTime.UtcNow;

					_dbcontext.SaveChanges();

				}

				return Json(new { exists = false, data = GetColors() });

			}
			catch (Exception ex)
			{
				_logger.LogError($"[Delete Color ERROR]!\n\n{ex.Message}");
				return Json(new
				{
					exists = false,
					error = true,
					message = $"An expected error occured while deleting color {existingColors.Name}!"
				});
			}

		}

	}
}
