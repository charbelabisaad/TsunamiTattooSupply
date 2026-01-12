using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	public class Licence
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string Code { get; set; }

		public DateTime ExpiryDate { get; set; }
		 
	}
}
