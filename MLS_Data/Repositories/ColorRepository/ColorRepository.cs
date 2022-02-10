using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.ColorRepository
{
    public class ColorRepository : GenericRepository<Color_DataModel>, IColorRepository
    {
        public ColorRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
