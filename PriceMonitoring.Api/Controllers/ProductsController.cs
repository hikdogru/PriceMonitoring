using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;
using System.Threading.Tasks;

namespace PriceMonitoring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region fields

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        #endregion

        #region ctor

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        #endregion

        #region methods

        [HttpGet(Name = "getallproducts")]
        public IActionResult GetAllProducts()
        {
            var result = _productService.GetProductListDtoAsSqlView();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}", Name = "getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost]
        public IActionResult Add(ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);
            var result = _productService.Add(product);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductUpdateDto productUpdateDto)
        {
            var productResult = _productService.GetById(id: id);
            if (!productResult.Success)
            {
                return BadRequest(productResult);
            }
            _mapper.Map(productUpdateDto, productResult.Data);
            var result = _productService.Update(productResult.Data);
            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(new Product { Id = id });
            if (result.Success)
            {
                return NoContent();
            }
            return BadRequest(result);

        }

        #endregion
    }
}
