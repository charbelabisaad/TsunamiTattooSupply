using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Subscriptions")]
	//[Index(nameof(Email), IsUnique = true)]
	public class Subscription
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(300)]
		public string Email { get; set; }

		public DateTime CreationDate { get; set; }
		 
	}
}
