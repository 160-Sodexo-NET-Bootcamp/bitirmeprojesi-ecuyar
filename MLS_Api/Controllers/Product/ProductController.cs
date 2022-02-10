using AutoMapper;
using Entity.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.UoW;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] RegisterProductDto productDto)
        {
            var product = mapper.Map<RegisterProductDto, Product_DataModel>(productDto);

            var result = await unitOfWork.Products.Add(product);

            if (result == false)
            {
                return BadRequest();
            }

            unitOfWork.Complete();
            return Ok();
        }
    }
}
