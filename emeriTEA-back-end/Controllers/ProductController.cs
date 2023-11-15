using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Data;
using emeriTEA_back_end.IServices;
using emeriTEA_back_end.Services;
using Microsoft.Extensions.Configuration;
using Entities;

namespace emeriTEA_back_end.Controllers
{

    [EnableCors("AllowAll")]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ServiceContext _serviceContext;

        public ProductController(IProductService productService, ServiceContext serviceContext)
        {

            _productService = productService;
            _serviceContext = serviceContext;
        }

        [HttpPost(Name = "InsertProduc")]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {

                var existingUserWithSameName = _serviceContext.Product.FirstOrDefault(u => u.Name_product == product.Name_product);

                if (existingUserWithSameName != null)
                {
                    return StatusCode(409, "A Product with the same name address already exists.");
                }
                else
                {

                    return Ok(_productService.InsertProduct(product));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting ID: {ex.Message}");
            }
        }

    }
}
