using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("GetWays")]
	public class GetWay
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[StringLength(50)]
		public string MerchantID { get; set; }

		[Column(TypeName = "text")]
		public string MerchantPassword { get; set; }

		[StringLength(4)]
		public string Mode { get; set; }

		[StringLength (150)]
		public string Url { get; set; }

		[Required]
		public string StatusID { get; set; }

		[ForeignKey("StatusID")]
		public virtual  Status Status { get; set; }	

	}
}
