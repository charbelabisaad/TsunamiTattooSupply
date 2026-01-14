using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("SiteSettings")]
	public class SiteSetting
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(50)]
		public string? Logo { get; set; }

		[StringLength(50)]
		public string? Icon {  get; set; }

		[StringLength(500)]
		public string ?CopyRight { get; set; }	
		 
		[Column(TypeName ="text")]
		public string? About { get; set; }

		  
		[Column(TypeName = "text")]
		public string? Services { get; set; }

		[Column(TypeName = "text")]
		public string? LicenceCode { get; set; }
		 
	}
}
