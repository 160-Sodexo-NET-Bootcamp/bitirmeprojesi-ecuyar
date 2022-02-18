using Entity.Shared;

namespace Entity.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
