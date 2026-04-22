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
		public IActionResult SendEmail(string Name, string Email, string Phone, string Message)
		{
			try
			{
				MailAddress mfrom = new MailAddress("info@tsunamitattoosupply.com", "Tsunami Tattoo Supply");

				// send TO yourself
				MailAddress mto = new MailAddress("info@tsunamitattoosupply.com");

				MailMessage mailMessage = new MailMessage(mfrom, mto);
				mailMessage.CC.Add("charbel.b.abisaad@gmail.com");
				mailMessage.Subject = "Contact Form Message";
				mailMessage.IsBodyHtml = true;

				mailMessage.Body = $@"
            <h3>New Contact Request</h3>
            <p><b>Name:</b> {Name}</p>
            <p><b>Email:</b> {Email}</p>
            <p><b>Phone:</b> {Phone}</p>
            <p><b>Message:</b><br/>{Message}</p>
        ";

				// Optional: reply to user email
				mailMessage.ReplyToList.Add(new MailAddress(Email));

				SmtpClient smtpclient = new SmtpClient("relay-hosting.secureserver.net", 25)
				{
					EnableSsl = false,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(
						"tsunamitattoosupply",
						Global.Decrypt("R0VPZXNwZXIwMzI3MzUxMSQ=")
					)
				};

				smtpclient.Send(mailMessage);

				return Json(new
				{
					success = true,
					message = "Your message has been sent successfully."
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message // keep for debugging
				});
			}
		}
	}
}
