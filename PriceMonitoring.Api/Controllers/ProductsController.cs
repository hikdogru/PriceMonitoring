using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.Entities.DTOs;

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



        [HttpGet(Name = "getallproducts")]
        public IActionResult GetAllProducts()
        {
            var result = _productService.GetAll();
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
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productCreateDto);
                var result = _productService.Add(product);
                if (result.Success)
                {
                    return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
                }
                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductUpdateDto productUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var isExistProductInDatabase = _productService.GetById(id: id);
                if (isExistProductInDatabase.Success)
                {
                    var product = _mapper.Map<Product>(productUpdateDto);
                    var result = _productService.Update(product);
                    if (result.Success)
                    {
                        return NoContent();
                    }
                }
                return BadRequest(isExistProductInDatabase.Message);

            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isExistProductInDatabase = _productService.GetById(id: id);
            if (isExistProductInDatabase.Success)
            {
                var result = _productService.Delete(isExistProductInDatabase.Data);
                if (result.Success)
                {
                    return NoContent();
                }
                return BadRequest(result.Message);                                                  
            }
            return NotFound();
        }
    }
}
