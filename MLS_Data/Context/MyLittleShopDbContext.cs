using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLS_Data.DataModels;

namespace MLS_Data.Context
{
    public class MyLittleShopDbContext : IdentityDbContext<User_DataModel>, IMyLittleShopDbContext
    { 
        public MyLittleShopDbContext(DbContextOptions<MyLittleShopDbContext> options) : base(options)
        {

        }

        //public DbSet<User_DataModel> Users { get; set; }
        public DbSet<Category_DataModel> Categories { get; set; }
        public DbSet<Product_DataModel> Products { get; set; }
    }
}
