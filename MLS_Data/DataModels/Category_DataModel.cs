using Entity.Shared;
using System.ComponentModel.DataAnnotations;

namespace MLS_Data.DataModels
{
    public class Category_DataModel : BaseEntity
    {
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
