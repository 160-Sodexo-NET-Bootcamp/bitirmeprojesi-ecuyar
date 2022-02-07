using Microsoft.EntityFrameworkCore;
using MLS_Data.DataModels;

namespace MLS_Data.Context
{
    public interface IMyLittleShopDbContext
    {
        DbSet<User_DataModel> Users { get; set; }
        DbSet<Category_DataModel> Categories { get; set; }
        DbSet<Product_DataModel> Products { get; set; }
    }
}
