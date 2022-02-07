namespace MLS_Data.DataModels
{
    public class Category_DataModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
