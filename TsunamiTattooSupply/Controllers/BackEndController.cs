using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TsunamiTattooSupply.Functions;
using System.Data;
using Npgsql;
using TsunamiTattooSupply.Data;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TsunamiTattooSupply.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace TsunamiTattooSupply.Controllers
{
		
	[AllowAnonymous]
	public class BackEndController : Controller
	{

		private readonly TsunamiDbContext _dbContext;
 
		IConfiguration _configuration;

		

		public BackEndController(IConfiguration configuration, TsunamiDbContext dbContext)
		{
			_configuration = configuration;
			_dbContext = dbContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LogIn(string Username, string Password)
		{
			try
			{
				bool changepassword = false;

				// ================================
				// 🔎 BASIC VALIDATION
				// ================================
				if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
				{
					return Json(new
					{
						success = false,
						message = "Please enter username and password!"
					});
				}

				string hashedPassword = ComputeSha256Hash(Password);

				// ================================
				// 🔍 USER CHECK
				// ================================
				var existingUser = _dbContext.Users
					.FirstOrDefault(u =>
						u.Username == Username &&
						u.Password == hashedPassword &&
						u.Status.ID == "A"
					);

				if (existingUser == null)
				{
					return Json(new
					{
						success = false,
						message = "Invalid username or password!"
					});
				}

				// ================================
				// 🔐 CLAIMS AUTH
				// ================================
				var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, existingUser.ID.ToString()),
			new Claim(ClaimTypes.Name, existingUser.Username),
			new Claim("FirstName", existingUser.FirstName ?? ""),
			new Claim("LastName", existingUser.LastName ?? "")
		};

				var identity = new ClaimsIdentity(
					claims,
					CookieAuthenticationDefaults.AuthenticationScheme
				);

				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties
					{
						IsPersistent = true,
						ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
					});

				// ================================
				// 🔑 FORCE CHANGE PASSWORD
				// ================================
				const string defaultHash =
					"2ea93d530707666bcd22829bc75556e2d4952ca403d5bfdadc76b9b6149f15df";

				if (existingUser.Password == defaultHash && hashedPassword == defaultHash)
				{
					changepassword = true;
				}

				return Json(new
				{
					success = true,
					changepassword
				});
			}
			catch (Exception ex)
			{
				// 🔥 LOG EXCEPTION (recommended)
				// _logger.LogError(ex, "Login failed");

				return Json(new
				{
					success = false,
					message = $"An unexpected error occurred. Please try again.\n\n{ ex.Message }"
				});
			}
		}

		[HttpPost]
		public IActionResult ChangePassword(string NewPassword, string ConfirmPassword)
		{
			if (NewPassword != ConfirmPassword)
				return Json(new { success = false, message = "Passwords do not match" });

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var user = _dbContext.Users.Find(int.Parse(userId));
			if (user == null)
				return Json(new { success = false, message = "User not found" });

			user.Password = ComputeSha256Hash(NewPassword);
		  

			_dbContext.SaveChanges();

			return Json(new { success = true });
		}


		[HttpPost]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme
			);

			Response.Cookies.Delete(".AspNetCore.Cookies");

			return RedirectToAction("Index", "BackEnd");
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
 
		 
	}
}
