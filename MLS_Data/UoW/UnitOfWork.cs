using MLS_Data.Context;
using MLS_Data.Repositories.BrandRepository;
using MLS_Data.Repositories.CategoryRepository;
using MLS_Data.Repositories.ColorRepository;
using MLS_Data.Repositories.OfferRepository;
using MLS_Data.Repositories.ProductRepository;
using MLS_Data.Repositories.SalesRepository;

namespace MLS_Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyLittleShopDbContext context;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IBrandRepository Brands { get; private set; }
        public IColorRepository Colors { get; private set; }
        public IOfferRepository Offers { get; private set; }
        public ISalesRepository Sales { get; private set; }

        public UnitOfWork(MyLittleShopDbContext context)
        {
            this.context = context;

            Categories = new CategoryRepository(context);
            Brands = new BrandRepository(context);
            Colors = new ColorRepository(context);
            Products = new ProductRepository(context);
            Offers = new OfferRepository(context);
            Sales = new SalesRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
