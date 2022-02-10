using MLS_Data.Repositories.BrandRepository;
using MLS_Data.Repositories.CategoryRepository;
using MLS_Data.Repositories.ColorRepository;
using MLS_Data.Repositories.ProductRepository;
using MLS_Data.Repositories.UserRepository;

namespace MLS_Data.UoW
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        IColorRepository Colors { get; }

        int Complete();
    }
}
