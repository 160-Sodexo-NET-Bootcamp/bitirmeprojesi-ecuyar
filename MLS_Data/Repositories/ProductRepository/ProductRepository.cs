﻿using Entity.Identity;
using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<int, Product_DataModel, ApplicationUser>, IProductRepository
    {
        public ProductRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
