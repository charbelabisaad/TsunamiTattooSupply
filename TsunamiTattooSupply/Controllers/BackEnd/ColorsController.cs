using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
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

				return Json(new { data = colors , success = true });

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
						StatusID = c.Status.ID,
						StatusDescription = c.Status.Description,
						StatusColor = c.Status.Color,
						IsCustom = c.IsCustom,

					})).ToList();
		
		}

		[HttpPost]
		public IActionResult SaveColors(int ID, string Code, string Name, bool IsCustom, string StatusID)
		{
			try
			{
				// Normalize
				Name = Name?.Trim();
				Code = Code?.Trim().Replace("#", string.Empty);

				// Validation
				if (!IsCustom && string.IsNullOrEmpty(Code))
				{
					return Json(new { error = true, message = "Color code is required." });
				}

				// Custom colors must NOT have a code
				if (IsCustom)
				{
					Code = null;
				}

				// ================= EDIT =================
				if (ID != 0)
				{
					var existingColor = _dbcontext.Colors.FirstOrDefault(c => c.ID == ID);

					if (existingColor == null)
					{
						return Json(new { error = true, message = "Color not found." });
					}

					var duplicate = _dbcontext.Colors
						.Any(c => c.Name.ToLower() == Name.ToLower()
							   && c.ID != ID
							   && c.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Color name already exists!" });
					}

					existingColor.Name = Name;
					existingColor.Code = Code;
					existingColor.IsCustom = IsCustom;
					existingColor.StatusID = StatusID;
					existingColor.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingColor.EditDate = DateTime.UtcNow;

					_dbcontext.SaveChanges();
				}
				// ================= CREATE =================
				else
				{
					var duplicate = _dbcontext.Colors
						.Any(c => c.Name.ToLower() == Name.ToLower()
							   && c.DeletedDate == null);

					if (duplicate)
					{
						return Json(new { exists = true, message = "Color name already exists!" });
					}

					var newColor = new Color
					{
						Name = Name,
						Code = Code,
						IsCustom = IsCustom,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow

					};

					_dbcontext.Colors.Add(newColor);
					_dbcontext.SaveChanges();
				}

				return Json(new
				{
					success = true,
					data = GetColors()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Save Color ERROR]");
				return Json(new
				{
					error = true,
					message = $"An unexpected error occurred while saving color {Name}!"
				});
			}
		}

		[HttpPost]
		public IActionResult DeleteColor(int ID)
		{
			try
			{
				var color = _dbcontext.Colors
					.FirstOrDefault(c => c.ID == ID && c.DeletedDate == null);

				if (color == null)
				{
					return Json(new
					{
						error = true,
						message = "Color not found or already deleted."
					});
				}

				int userId = 0;
				int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

				color.DeletedUserID = userId;
				color.DeletedDate = DateTime.UtcNow;

				_dbcontext.SaveChanges();

				return Json(new
				{
					success = true,
					data = GetColors()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[Delete Color ERROR]");
				return Json(new
				{
					error = true,
					message = "An unexpected error occurred while deleting the color."
				});
			}
		}


	}
}
