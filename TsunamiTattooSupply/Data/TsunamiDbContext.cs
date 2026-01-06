
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
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<FilePath> FilePaths { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<ClientAddress> ClientAddresses { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<CurrencyConversion> CurrenciesConversion {  get; set; } 
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductSubCategory> ProductsSubCategories { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<ProductSize> ProductsSizes { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<ProductColor> ProductsColors { get; set; }
		public DbSet<ProductImage> ProductsImages { get; set; }		
		public DbSet<Stock> Stocks { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<GetWay> GetWays { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<TrackingOrderStatus> TrackingOrderStatuses { get; set; }
		public DbSet<TrackingOrder> TrackingOrders { get; set; }
		public DbSet<ProductBestSeller> ProductsBestSeller { get; set; }
		public DbSet<ProductRecentlyViewed> ProductsRecentlyViewed { get; set; }
		public DbSet<ProductWishList> ProductsWishList { get; set; }
		public DbSet<SocialMedia> SocialMedias { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<POSStock> POSStocks { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Banner> Banners { get; set; }
		public DbSet<BannerMobile> BannersMobiles { get; set; }
		public DbSet<BannerPage> BannersPages { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<GroupType> GroupTypes { get; set; }

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
