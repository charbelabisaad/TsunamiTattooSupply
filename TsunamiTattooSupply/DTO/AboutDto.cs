using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class AboutDto
	{ 
		public string ID { get; set; }
		 
		public string ShortText { get; set; }
		 
		public string LongText { get; set; }
		 
		public string? Image { get; set; }

	}
}
