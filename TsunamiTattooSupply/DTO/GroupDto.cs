using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class GroupDto
	{
	 
		public int ID { get; set; }
		 
		public string Name { get; set; }
		 
		public string? Summary { get; set; }
		 
		public string ImagePath { get; set; }

		public string? Image { get; set; }
		 
		public bool ShowHome { get; set; } = true;
		 
		public int Rank { get; set; } = 0;

		public string TypeID { get; set; }

		public string TypeDescription { get; set; }
 
		public string StatusID { get; set; }

		public string Status {  get; set; }

		public string StatusColor { get; set; }
		 
	}
}
