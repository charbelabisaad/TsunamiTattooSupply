using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.DTO;
using TsunamiTattooSupply.Functions;
using TsunamiTattooSupply.ViewModels;

namespace TsunamiTattooSupply.Controllers.FrontEnd
{
	public class ContactUsController : Controller
	{
		private readonly TsunamiDbContext _dbContext;
		private readonly ILogger<ContactUsController> _logger;
		private readonly string _imagesRoot;

		public ContactUsController(TsunamiDbContext dbContext, ILogger<ContactUsController> logger, IConfiguration config)
		{
			_dbContext = dbContext;
			_logger = logger;
			_imagesRoot = config["StacticFiles:imagesRoot"];
		}

		public IActionResult Index()
		{
			FilePathService fp = new FilePathService(_dbContext);

			Global.BannerPageWebImagePath = fp.GetFilePath("BNNRPGEWBIMG").Description;

			var vm = new PageViewModel
			{
				bannercontact = GetBannerPages("WEB", "CNTCT"),
				contact = GetContact("CNTCT")

			};

			return View("~/Views/FrontEnd/ContactUs/Index.cshtml", vm);
		}

		public ContactDto GetContact(string ID)
		{
			return _dbContext.Contacts
				.Where(c => c.ID == ID)
				.Select(c => new ContactDto
				{
					ID = c.ID,
					Phone = c.Phone,
					Email = c.Email,
					Location = c.Location,
					LocationMapLink = c.LocationMapLink,
				}).FirstOrDefault();
		}

		public BannerPageDto GetBannerPages(string AppType, string PageLocationCode)
		{
			return _dbContext.BannersPages.Where(bp => bp.AppType == AppType
													&& bp.PageLocation.Code == PageLocationCode
													&& bp.StatusID == "A"
													&& bp.DeletedDate == null)
										  .Select(bp => new BannerPageDto
										  {
											  ID = bp.ID,
											  Name = bp.Name,
											  ImagePath = Global.BannerPageWebImagePath,
											  Image = bp.Image
										  }
										).FirstOrDefault();
		}
		[HttpPost]
		public IActionResult SendEmail(string Name, string Email, string Phone, string Subject, string Message)
		{
			try
			{
				var smtpClient = new SmtpClient("mail.tsunamitattoosupply.com")
				{
					Port = 587,
					Credentials = new NetworkCredential("info@tsunamitattoosupply.com", ""),
					EnableSsl = true
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress("info@tsunamitattoosupply.com", "Tsunami Tattoo Supply"),
					Subject = "Contact Form - " + Subject,
					Body = $@"
                <h3>New Contact Request</h3>
                <p><strong>Name:</strong> {Name}</p>
                <p><strong>Email:</strong> {Email}</p>
                <p><strong>Phone:</strong> {Phone}</p>
                <p><strong>Subject:</strong> {Subject}</p>
                <p><strong>Message:</strong><br/>{Message}</p>
            ",
					IsBodyHtml = true
				};

				mailMessage.To.Add("info@tsunamitattoosupply.com");
				mailMessage.CC.Add("charbel.b.abisaad@gmail.com");

				smtpClient.Send(mailMessage);

				return Json(new
				{
					success = true,
					message = "Email sent successfully"
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
