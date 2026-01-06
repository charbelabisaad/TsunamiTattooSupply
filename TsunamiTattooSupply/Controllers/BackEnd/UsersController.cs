using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Models; 
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text; 
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace TsunamiTattooSupply.Controllers.BackEnd
{
	[Authorize]
	public class UsersController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<UsersController> _logger;

		public UsersController(TsunamiDbContext dbContext, ILogger<UsersController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public List<UserType> usertypes { get; set; }
		public List<Role> roles { get; set; }
		public List<User> users { get; set; }

		public IActionResult Index()
		{
			usertypes = GetUserTypes();
			roles  = GetRoles();
			ViewBag.UserTypes = usertypes;
			ViewBag.Roles = roles;	
			return View("~/Views/BackEnd/Users/Index.cshtml");

		}

		[HttpGet]
		public List<UserType> GetUserTypes()
		{

			List<UserType> userTypesList = new List<UserType>();

			userTypesList = _dbContext.UserTypes.OrderBy(ut => ut.Description).ToList();

			return userTypesList;

		}

		[HttpGet]
		public List<Role> GetRoles() { 
		
			List<Role> rolesList = new List<Role>();

			rolesList = _dbContext.Roles
				.Where(r => r.IsAdmin == false)
				.OrderBy(r => r.Description ).ToList();

			return rolesList;
		
		}

		[HttpGet]
		public IActionResult ListGetUsers()
		{
			try
			{
				var users = GetUsers();
				return Json(new { data = users, success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Fetching Users![ERROR].");

				return Json(new
				{
					data = new List<object>(),
					success = false,
					message = "An unexpected error occurred while loading users!"
				});
			}
		}
		 
		[HttpPost]
		public IActionResult SaveUser(int ID, string Username, string FirstName, string LastName, string UserTypeID, int RoleID, string StatusID)
		{

			string normalizedUsername = Username?.Trim().ToLower();
			string normalizedFirstName = FirstName?.Trim().ToUpper();
			string normalizedLastName = LastName?.Trim().ToUpper();

			int UserID = 0;

			try
			{
				// Normalize input values


				if (string.IsNullOrEmpty(normalizedUsername) ||
					string.IsNullOrEmpty(normalizedFirstName) ||
					string.IsNullOrEmpty(normalizedLastName))
				{
					return Json(new { exists = false, error = true, message = "Missing required fields." });
				}

				// ✅ Edit Mode
				if (ID != 0)
				{
					var existingUser = _dbContext.Users.FirstOrDefault(u => u.ID == ID);
					if (existingUser == null)
					{
						return Json(new { exists = false, error = true, message = "User not found." });
					}

					// Check duplicate username for others
					var existingUsername = _dbContext.Users
						.FirstOrDefault(u => u.Username.ToLower() == normalizedUsername && u.ID != ID);

					if (existingUsername != null)
					{
						return Json(new { exists = true, message = "Username already exists!" });
					}

					// Update fields
					existingUser.Username = normalizedUsername.Trim().ToLower();
					existingUser.FirstName = normalizedFirstName.Trim().ToUpper();
					existingUser.LastName = normalizedLastName.Trim().ToUpper();
					existingUser.UserTypeID = UserTypeID;
					existingUser.RoleID = RoleID;
					existingUser.StatusID = StatusID;
					existingUser.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
					existingUser.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

					UserID = ID;

				}
				else // ✅ New User
				{
					// Check duplicate username
					var existingUsername = _dbContext.Users
						.FirstOrDefault(u => u.Username.ToLower() == normalizedUsername);

					if (existingUsername != null)
					{
						return Json(new { exists = true, message = "Username already exists!" });
					}

					var newUser = new User
					{
						Username = normalizedUsername.Trim().ToLower(),
						FirstName = normalizedFirstName.Trim().ToUpper(),
						LastName = normalizedLastName.Trim().ToUpper(),
						Password = "2ea93d530707666bcd22829bc75556e2d4952ca403d5bfdadc76b9b6149f15df",
						UserTypeID = UserTypeID,
						RoleID = RoleID,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Users.Add(newUser);
					_dbContext.SaveChanges();

					UserID = newUser.ID;

				}

				// Return updated list for DataTable


				return Json(new { exists = false, users = GetUsers() });
			}
			catch (Exception ex)
			{
				// Log the error (optional)
				_logger.LogError($"[Save User ERROR]!\n\n{ex.Message}");

				return Json(new
				{
					exists = false,
					error = true,
					message = $"An unexpected error occurred while saving the user {Username}!"
				});
			}
		}

		private string ComputeSha256Hash(string input)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = sha256.ComputeHash(bytes);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		public List<UserDto> GetUsers()
		{
			List<UserDto> users = new List<UserDto>();

			try {  
			  
				  users = _dbContext.Users
					.Where(u => u.DeletedDate == null && u.Role.IsAdmin == false)
					.Include(u => u.UserType)
					.Include (u => u.Role)
					.Include(u => u.Status)
					.Select(u => new UserDto
					{
						Id = u.ID,
						Username = u.Username,
						FirstName = u.FirstName,
						LastName = u.LastName,
						UserTypeID = u.UserType.ID,
						UserType = u.UserType.Description,
						UserRoleID = u.Role.ID,
						UserRole = u.Role.Description,
						StatusID = u.Status.ID,
						Status = u.Status.Description,
						StatusColor = u.Status.Color

					})
					.ToList(); // explicit cast to List<object>
			}
			catch (Exception ex)
			{
				users = null;
				_logger.LogError(ex, "Fetch Users [ERROR]!");
			}

			return users;
		}

		[HttpPost]
		public IActionResult DeleteUser(int id)
		{	
			var user = _dbContext.Users.FirstOrDefault(u => u.ID == id);

			try
			{
			
				if (user == null)
					return Json(new { success = false, message = "User not found." });

				// 🟡 Soft delete logic
				user.DeletedDate = DateTime.UtcNow;
				user.DeletedUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				_dbContext.SaveChanges();

				return Json(new { success = true, users = GetUsers() });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete User [ERROR]!");
				return Json(new { 
					success = false, message = $"An unexpected error occurred while deleting user {user.Username}!" });
			}
		}

		[HttpPost]
		public IActionResult ResetPassword(int id)
		{
			try
			{
				var user = _dbContext.Users.FirstOrDefault(u => u.ID == id);
				if (user == null)
				{
					return Json(new { success = false, message = "User not found." });
				}

				// Example: reset to default password (hashed if you use hashing)
				user.Password = "2ea93d530707666bcd22829bc75556e2d4952ca403d5bfdadc76b9b6149f15df"; // ⚠️ For testing only
				user.EditUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
				user.EditDate = DateTime.UtcNow;

				_dbContext.SaveChanges();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Reset Password [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while resetting password for user {id}!" });
			}
		}

	}

}
