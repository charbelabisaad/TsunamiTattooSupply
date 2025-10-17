using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Index(nameof(Description), IsUnique = true)]
	public class FilePath
	{
		[Key]
		[Required, StringLength(20)]
		public string Code { get; set; }

		[Required, Column(TypeName = "text")]
		public string Description { get; set; }
		 
	}
}
