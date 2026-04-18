using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class AboutDto
	{ 
		public string ID { get; set; }
		 
		public string ShortText1 { get; set; }

		public string ShortText2 { get; set; }
		 
		public string LongText { get; set; }
		 
		public string ImagePath { get; set; }

		public string? Image1 { get; set; }
		public string? Image2 { get; set; }
		 
	}
}
