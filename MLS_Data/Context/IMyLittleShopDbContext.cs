using Microsoft.EntityFrameworkCore;
using MLS_Data.DataModels;

namespace MLS_Data.Context
{
    public interface IMyLittleShopDbContext
    {
        DbSet<Category_DataModel> Categories { get; set; }
        DbSet<Brand_DataModel> Brands { get; set; }
        DbSet<Color_DataModel> Colors { get; set; }
        DbSet<Product_DataModel> Products { get; set; }
        DbSet<ApplicationUser_DataModel> ApplicationUsers { get; set; }
        DbSet<Offer_DataModel> Offers { get; set; }
        DbSet<Sales_DataModel> Sales { get; set; }
    }
}
