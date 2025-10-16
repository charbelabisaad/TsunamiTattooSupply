
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TsunamiTattooSupply.Models; 
namespace TsunamiTattooSupply.Data
{
	public class TsunamiDbContext :DbContext
	{
		public TsunamiDbContext(DbContextOptions<TsunamiDbContext> options) : base(options) { }

		public DbSet<Status> Statuses { get; set; }
		public DbSet<UserType> UserTypes { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Role>()
		   .HasOne(r => r.CreatedUser)
		   .WithMany()
		   .HasForeignKey(r => r.CreatedUserID)
		   .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Role>()
				.HasOne(r => r.EditUser)
				.WithMany()
				.HasForeignKey(r => r.EditUserID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Role>()
				.HasOne(r => r.DeletedUser)
				.WithMany()
				.HasForeignKey(r => r.DeletedUserID)
				.OnDelete(DeleteBehavior.Restrict);

		}
		 
	}
	 
}
