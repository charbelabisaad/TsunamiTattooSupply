using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("TrackingOrderStatuses")]
	[Index(nameof(Code), IsUnique = true)]
	[Index(nameof(Description), IsUnique = true)]
	public class TrackingOrderStatus
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "char(3)")]
		public string Code { get; set; }

		[Required]
		[StringLength(50)]
		public string Description { get; set; }
		 
	}
}
