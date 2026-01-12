using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ContactTypes")]
	public class ContactType
	{
		[Key]
		public int ID { get; set; }
		public string Description { get; set; }
	}
}
