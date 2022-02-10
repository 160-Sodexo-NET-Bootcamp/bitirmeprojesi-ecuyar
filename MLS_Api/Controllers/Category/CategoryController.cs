using AutoMapper;
using Entity.Category;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryProducts(int categoryId)
        {
            //user will get chosen category's products
            var products = await unitOfWork.Categories.GetById(categoryId);
            var categoryDto = mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }
    }
}
