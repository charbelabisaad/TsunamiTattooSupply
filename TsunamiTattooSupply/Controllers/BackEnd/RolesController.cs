using Microsoft.AspNetCore.Mvc;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
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
				_logger.LogError(ex, "Fetching Users![ERROR].");

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

	}

}
