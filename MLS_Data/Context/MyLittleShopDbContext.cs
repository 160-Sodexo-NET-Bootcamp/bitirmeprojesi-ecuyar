using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLS_Data.DataModels;

namespace MLS_Data.Context
{
    public class MyLittleShopDbContext : IdentityDbContext<ApplicationUser_DataModel>, IMyLittleShopDbContext
    {
        public MyLittleShopDbContext(DbContextOptions<MyLittleShopDbContext> options) : base(options)
        {

        }

        public DbSet<Category_DataModel> Categories { get; set; }
        public DbSet<Brand_DataModel> Brands { get; set; }
        public DbSet<Color_DataModel> Colors { get; set; }
        public DbSet<Product_DataModel> Products { get; set; }
        public DbSet<ApplicationUser_DataModel> ApplicationUsers { get; set; }
        public DbSet<Offer_DataModel> Offers { get; set; }
        public DbSet<Sales_DataModel> Sales { get; set; }
    }
}
