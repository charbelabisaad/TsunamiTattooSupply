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
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			{
				ViewBag.Error = "Please enter username and password!";
				return View("Index");
			}

			var existingUser = _dbContext.Users.FirstOrDefault(u =>
				u.Username == Username &&
				u.Password == ComputeSha256Hash(Password) &&
				u.Status.ID == "A"
			);

			if (existingUser == null)
			{
				ViewBag.Error = "Invalid username or password!";
				return View("Index");
			}

			// ================================
			// 🔐 CLAIMS ONLY (NO COOKIES)
			// ================================
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, existingUser.ID.ToString()),
				new Claim(ClaimTypes.Name, existingUser.Username),

				new Claim("FirstName", existingUser.FirstName ?? ""),
				new Claim("LastName", existingUser.LastName ?? ""), 
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

			return RedirectToAction("Index", "Dashboard");
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
