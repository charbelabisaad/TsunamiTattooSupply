using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Controllers.BackEnd
{

	[Authorize]
	public class ClientsController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		
		public ILogger<ClientsController> logger { get; set; }

		public ClientsController(TsunamiDbContext dbContext, ILogger<ClientsController> logger)
		{
			_dbContext = dbContext;
			this.logger = logger;
		}	

		public IActionResult Index()
		{

			return View("~/Views/BackEnd/Clients/Index.cshtml");
		}

		[HttpGet]
		public IActionResult ListGetClients()
		{
			try
			{
				var clients = GetClients();
				return Json(new { data = clients, success = true });
			}
			catch (Exception ex)
			{

				return Json(new
				{
					data = new List<Client>(),
					success = false,
					message = "An unexpected error occurred while loading clients"
				});

			}

		}

		public List<ClientsDto> GetClients()
		{
			return _dbContext.Clients
				.Include(c => c.Country)
				.OrderBy(c => c.Name)
				.Select(c => new ClientsDto
				{
					ID = c.ID,
					Name = c.Name,
					Email = c.Email,
					PhoneNumber = (c.Country != null ? c.Country.Code + " " : "") + c.PhoneNumber
				})
				.ToList();
		}

		public IActionResult GetStatistics()
		{
			try
			{
				var statistics = _dbContext.Statistics
					.FirstOrDefault(c => c.Code == "CLNT");

				return Json(new
				{
					success = true,
					isCalculated = statistics == null ? false : statistics.IsCalculated,
					number = statistics == null ? 0 : statistics.Number
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					isCalculated = false,
					number = 0,
					message = ex.Message
				});
			}
		}

		[HttpPost]
		public IActionResult SaveStatistics(int number, bool isCalculated)
		{
			try
			{
				int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

				var statistic = _dbContext.Statistics
					.FirstOrDefault(x => x.Code == "CLNT");

				if (statistic == null)
				{
					statistic = new Statistic
					{
						Code = "CLNT",
						Name = "Client",
						Number = number,
						IsCalculated = isCalculated,
						StatusID = "A",
						CreatedUserID = userId,
						CreationDate = DateTime.UtcNow
					};

					_dbContext.Statistics.Add(statistic);
				}
				else
				{
					statistic.Number = number;
					statistic.IsCalculated = isCalculated;
					statistic.EditUserID = userId;
					statistic.EditDate = DateTime.UtcNow;
				}

				_dbContext.SaveChanges();

				return Json(new
				{
					success = true,
					message = "Clients number saved successfully"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}
		}

	}
}
