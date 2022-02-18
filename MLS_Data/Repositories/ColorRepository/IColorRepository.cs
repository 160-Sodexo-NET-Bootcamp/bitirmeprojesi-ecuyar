using Entity.Identity;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.ColorRepository
{
    public interface IColorRepository : IGenericRepository<int, Color_DataModel, ApplicationUser_DataModel>
    {
    }
}
