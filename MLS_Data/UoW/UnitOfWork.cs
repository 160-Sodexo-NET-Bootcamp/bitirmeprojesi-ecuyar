using MLS_Data.Context;
using MLS_Data.Repositories.BrandRepository;
using MLS_Data.Repositories.CategoryRepository;
using MLS_Data.Repositories.ColorRepository;
using MLS_Data.Repositories.OfferRepository;
using MLS_Data.Repositories.ProductRepository;

namespace MLS_Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyLittleShopDbContext context;
        //public IUserRepository Users { get; private set; }
        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IBrandRepository Brands { get; private set; }
        public IColorRepository Colors { get; private set; }
        public IOfferRepository Offers { get; private set; }

        public UnitOfWork(MyLittleShopDbContext context)
        {
            this.context = context;

            //Users = new UserRepository(context);
            Categories = new CategoryRepository(context);
            Brands = new BrandRepository(context);
            Colors = new ColorRepository(context);
            Products = new ProductRepository(context);
            Offers = new OfferRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
