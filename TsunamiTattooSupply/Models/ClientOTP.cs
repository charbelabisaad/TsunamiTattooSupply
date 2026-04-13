using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{

	[Table("ClientsOTP")] 
	public class ClientOTP
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		 
		[Required]
		public string Email { get; set; }

		[Required]
		[StringLength(6)]
		public string OTP {  get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ClientID")]
		public virtual Client Client { get; set; }

	}
}
