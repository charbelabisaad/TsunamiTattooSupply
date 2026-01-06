using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class RolesController : Controller
	{
		public readonly TsunamiDbContext _dbContext;
		public ILogger<RolesController> _logger;

		public RolesController(TsunamiDbContext dbContext, ILogger<RolesController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public List<Role> roles { get; set; }

		public IActionResult Index()
		{
			return View("~/Views/BackEnd/Roles/Index.cshtml");
		}

		public IActionResult ListGetRoles()
		{
			try
			{
				var roles = GetRoles();
				return Json(new { data = roles, success = true });
			}
			catch (Exception ex) {
				_logger.LogError(ex, "Fetching Roles![ERROR].");

				return Json(new
				{
					data = new List<object>(),
					success = false,
					message = "An unexpected error occurred while loading roles!"
				});
			}

		}

		public List<RoleDto> GetRoles()
		{

			List<RoleDto> roles = new List<RoleDto>();

			try
			{



				roles = _dbContext.Roles
					.Where(r => r.DeletedDate == null && r.IsAdmin == false)
					.OrderBy(r => r.Description)
					.Select(r => new RoleDto
					{
						ID = r.ID,
						Description = r.Description,
						IsAdmin = r.IsAdmin,
						StatusID = r.Status.ID,
						Status = r.Status.Description,
						StatusColor = r.Status.Color

					}).ToList();
			}
			catch (Exception ex) {
				roles = null;
				_logger.LogError(ex, "Fetch Roles [ERROR]!");
			}
			return roles;

		}

		[HttpPost]
		public IActionResult SaveRole(int ID, string Description, string StatusID)
		{
  
			int RoleID;

		try { 

			if (ID != 0)
			{
				var existingRole = _dbContext.Roles.FirstOrDefault(r => r.ID == ID);

				var existingRoleDescription =_dbContext.Roles.FirstOrDefault(r => r.Description.Trim().ToLower() == Description.Trim().ToLower() && r.ID != ID && r.DeletedDate == null);

				if (existingRoleDescription != null) {

					return Json(new { exists = true, message = "Description already exists!" });

				}

				existingRole.Description = Description;
				existingRole.StatusID = StatusID;
				existingRole.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				existingRole.EditDate = DateTime.UtcNow;

				_dbContext.SaveChanges();

				RoleID = ID;

			}
			else
			{
				var existingRoleDescription = _dbContext.Roles.FirstOrDefault(r => r.Description.Trim().ToLower() == Description.Trim().ToLower());

				if (existingRoleDescription != null)
				{

					return Json(new { exists = true, message = "Description already exists!" });

				}

				var newRole = new Role
				{
					Description = Description,
					StatusID = StatusID,
					CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
					CreationDate = DateTime.UtcNow

				}; 

				_dbContext.Roles.Add(newRole);

				_dbContext.SaveChanges();

				RoleID = newRole.ID;

			}
			
				return Json(new { exists = false, roles = GetRoles() });

			}
			catch (Exception ex)
			{
				// Log the error (optional)
				_logger.LogError($"[Save Role ERROR]!\n\n{ex.Message}");

				return Json(new
				{
					exists = false,
					error = true,
					message = $"An unexpected error occurred while saving the role {Description}!"
				});
			}
		}

		public IActionResult DeleteRole (int ID)
		{
			var existingRole = _dbContext.Roles.FirstOrDefault(r => r.ID == ID);

			try { 

				if (existingRole != null)
				{

					existingRole.DeletedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingRole.DeletedDate = DateTime.UtcNow;
					_dbContext.SaveChanges();

				}
			 
				return Json(new { success = true });

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete Role [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while deleting role {existingRole.Description}!" });
			}
		}
		 
	}

}
