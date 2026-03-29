using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class BannerDto
	{
		 
		public int ID { get; set; }
		 
		public string Description { get; set; }
		 
		public string ImagePath { get; set; }

		public string Image { get; set; }
		 
		public string? Sentence { get; set; }
		 
		public string? Link { get; set; }
		 
		public int Rank { get; set; }

		public string StatusID { get; set; }

		public string Status { get; set; }

		public string StatusColor { get; set; }


	}
}
