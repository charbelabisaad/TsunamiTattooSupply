using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TsunamiTattooSupply.Functions;
using System.Data;
using Npgsql; 

namespace TsunamiTattooSupply.Controllers
{
		

	public class BackEndController : Controller
	{
 
		IConfiguration _configuration;

		

		public BackEndController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(string Username, string Password)
		{
			// TODO: validate credentials (ADO.NET or EF)

			//if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
			//{
			//	ViewBag.Error = "Please enter username and password!";
			//	return View(); // Return to login view
			//}

			//bool isValidUser = IsValidUser(Username, Global.Encrypt(Password)); // replace with actual validation

			//if (isValidUser)
			//{
			HttpContext.Response.Cookies.Append("UserID", "0");
				return RedirectToAction("Index", "Dashboard");
			//}
			//else
			//{

			//	ViewBag.Error = "Invalid username or password!";
			//	return View(); // will return BackEnd login view
			//}
		}

		private bool IsValidUser(string USR_NAME, string USR_PASSWORD)
		{
			bool isValid = false;

			using (var DBConn = new NpgsqlConnection(_configuration.GetConnectionString("TsunamiConnection")))
			{
				DBConn.Open();

				using (var transaction = DBConn.BeginTransaction()) // <- start transaction
				{
					try
					{
						using (var cmd = new NpgsqlCommand("user_log_in", DBConn, transaction))
						{
							cmd.CommandType = CommandType.StoredProcedure;

							cmd.Parameters.AddWithValue("p_usr_name", USR_NAME);
							cmd.Parameters.AddWithValue("p_usr_password", USR_PASSWORD);
							cmd.Parameters.AddWithValue("p_usr_default", true);

							var refCursor = new NpgsqlParameter("ref", NpgsqlTypes.NpgsqlDbType.Refcursor);
							refCursor.Direction = ParameterDirection.InputOutput;
							refCursor.Value = "my_cursor";
							cmd.Parameters.Add(refCursor);

							// Execute procedure
							cmd.ExecuteNonQuery();
						}

						// Fetch the cursor in the same transaction
						using (var fetchCmd = new NpgsqlCommand("FETCH ALL FROM my_cursor;", DBConn, transaction))
						using (var rdr = fetchCmd.ExecuteReader())
						{
							if (rdr.Read())
								isValid = true; // user found
						}

						// Close cursor
						using (var closeCmd = new NpgsqlCommand("CLOSE my_cursor;", DBConn, transaction))
							closeCmd.ExecuteNonQuery();

						transaction.Commit(); // commit transaction
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						isValid = false;
						ViewData["ErrorHandling"] = "Error Log In\n\n" + ex.Message;
					}
				}

				DBConn.Close();
			}

			return isValid;
		}
		 
	}
}
