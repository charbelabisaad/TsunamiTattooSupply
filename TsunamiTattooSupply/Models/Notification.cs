using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Notifications")]
	[Index(nameof(ClientID))]
	public class Notification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "date")]
		public DateTime Date {  get; set; }

		[Required]
		public int ClientID { get; set; }

		[Required]
		public bool IsRead { get; set; } = false;

		[Required]
		[Column(TypeName = "text")]
		public string Message { get; set; }

		[ForeignKey("ClientID")]
		public virtual Client Client { get; set; }

	}
}
