using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("PageLocations")]
	public class PageLocation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[StringLength (50)]	
		public string Code { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string StatusID { get; set; }

		[ForeignKey("StatusID")]
		public virtual Status Status { get; set; }
		 
	}
}

