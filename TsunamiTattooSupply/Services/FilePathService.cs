using System.IO;
using TsunamiTattooSupply.Data;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.Functions
{
	public class FilePathService
	{
		private readonly TsunamiDbContext _dbContext;

		public FilePathService(TsunamiDbContext dbContext) { 
		
			_dbContext = dbContext;
		
		}

		public FilePath GetFilePath(string Code)
		{
			var filepath = _dbContext.FilePaths.FirstOrDefault(p => p.Code == Code);
			 
			return filepath;

		}

	}
}
