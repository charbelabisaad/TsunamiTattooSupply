using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Clients")] 
	[Index(nameof(PhoneCountryID))]   
	public class Client
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string Password { get; set; }
		 
		[Column(TypeName = "char(10)")]
		public string? PasswordMobile { get; set; }
 
		[Required]
		[StringLength(200)]
		public string Email { get; set; }

		[Required]
		public int PhoneCountryID { get; set; }

		[Required]
		[StringLength(50)]
		public string PhoneNumber { get; set; }

		[StringLength(255)]
		public string? DeviceToken { get; set; }
		  
		[Column(TypeName = "text")]
		public string? VerificationCode {  get; set; }

		[Column(TypeName = "text")]
		public string? ResetPasswordLink { get; set; }

		public bool Approvement { get; set; } = false;

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("PhoneCountryID")]
		public virtual Country Country { get; set; }
		 
		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
