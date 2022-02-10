using AutoMapper;
using Entity.Category;
using Entity.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.UoW;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            //main categories have a 0 value on parentcategoryid.
            //so we dont want main categories. we want subcategories
            var categories = await unitOfWork.Categories.GetAll();
            var categoryDtos = mapper.Map<IEnumerable<Category_DataModel>, List<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [HttpGet("products")]
        public IActionResult GetCategoryProducts([FromQuery] int categoryId)
        {
            //user will get chosen category's unsold products
            var products = unitOfWork.Products.Where(x => x.CategoryId == categoryId && x.IsSold == false);
            var productsDto = mapper.Map<List<Product_DataModel>, List<ShowProductDto>>(products);

            return Ok(productsDto);
        }
    }
}
