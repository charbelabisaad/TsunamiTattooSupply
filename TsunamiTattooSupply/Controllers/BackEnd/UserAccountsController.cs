using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsunamiTattooSupply.Models; 
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text; 
using TsunamiTattooSupply.Data;

namespace TsunamiTattooSupply.Controllers.BackEnd
{
	public class UserAccountsController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<UserAccountsController> _logger;
		 
		public UserAccountsController(TsunamiDbContext dbContext, ILogger<UserAccountsController> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}
		 
		public List<UserType> usertypes { get; set; }
		public List<User> users { get; set; }

		public IActionResult Index()
		{
			usertypes = GetUserTypes(); 
			ViewBag.UserTypes = usertypes;
			return View("~/Views/BackEnd/UserAccounts/Index.cshtml");
			 
		}

		[HttpGet]
		public List<UserType> GetUserTypes()
		{

			List<UserType> userTypesList = new List<UserType>();
			 
			userTypesList = _dbContext.UserTypes.OrderBy(ut => ut.Description).ToList();

			return userTypesList;

		}

		[HttpGet]
		public IActionResult ListGetUsers()
		{
			try
			{
				var users = GetUsers();
				return Json(new { data = users });
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
		public IActionResult SavePost(int ID, string Username, string FirstName, string LastName, string UserTypeID, string StatusID)
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
					existingUser.Username = normalizedUsername;
					existingUser.FirstName = normalizedFirstName;
					existingUser.LastName = normalizedLastName;
					existingUser.UserTypeID = UserTypeID;
					existingUser.StatusID = StatusID;
					existingUser.EditUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
					existingUser.EditDate = DateTime.UtcNow;

					_dbContext.SaveChanges();

					UserID =  ID;

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
						Username = normalizedUsername,
						FirstName = normalizedFirstName,
						LastName = normalizedLastName,
						Password = "a9a575415e09850e33fc573b1738415ec23ac1aea3dad08a9fb0eb602499d1f6",
						UserTypeID = UserTypeID,
						StatusID = StatusID,
						CreatedUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]),
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Users.Add(newUser);
					_dbContext.SaveChanges();

					UserID = newUser.ID;

				}

				// Return updated list for DataTable
			 

				return Json(new { exists = false, data = GetUsers() });
			}
			catch (Exception ex)
			{
				// Log the error (optional)
				_logger.LogError($"[Save User ERROR]!\n\n{ex.Message}");

				return Json(new
				{
					exists = false,
					error = true,
					message = $"An unexpected error occurred while saving the user {UserID}!"
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

		public IEnumerable<object> GetUsers()
		{
			List<object> users = new();

			try
			{
				users = _dbContext.Users
					.Where(u => u.DeletedDate == null)
					.Include(u => u.UserType)
					.Include(u => u.Status)
					.Select(u => new
					{
						id = u.ID,
						username = u.Username,
						firstName = u.FirstName,
						lastName = u.LastName,
						userTypeID = u.UserType.ID,
						userType = u.UserType.Description,
						statusID = u.Status.ID,
						status = u.Status.Description
					})
					.ToList<object>(); // explicit cast to List<object>
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
			try
			{
				var user = _dbContext.Users.FirstOrDefault(u => u.ID == id);
				if (user == null)
					return Json(new { success = false, message = "User not found." });

				// 🟡 Soft delete logic
				user.DeletedDate = DateTime.UtcNow;
				user.DeletedUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);

				_dbContext.SaveChanges();

				return Json(new { success = true, users = GetUsers() });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Delete User [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while deleting user {id}!" });
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
				user.Password = "a9a575415e09850e33fc573b1738415ec23ac1aea3dad08a9fb0eb602499d1f6"; // ⚠️ For testing only
				user.EditUserID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
				user.EditDate = DateTime.UtcNow;

				_dbContext.SaveChanges();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,"Reset Password [ERROR]!");
				return Json(new { success = false, message = $"An unexpected error occurred while resetting password for user {id}!" });
			}
		}

	}

}
