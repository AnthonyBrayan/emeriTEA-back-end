using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Data;
using emeriTEA_back_end.IServices;
using emeriTEA_back_end.Services;
using Microsoft.Extensions.Configuration;
using Entities;
using System.Security.Authentication;

namespace emeriTEA_back_end.Controllers
{

    [EnableCors("AllowAll")]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ITokenService _tokenService;
        private readonly ServiceContext _serviceContext;

        public ProductController(IProductService productService, ITokenService tokenService, ServiceContext serviceContext)
        {

            _productService = productService;
            _tokenService = tokenService;
            _serviceContext = serviceContext;

        }

        [HttpPost(Name = "InsertProduc")]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                //var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
                var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);
                if (userId == null)
                {

                    return Unauthorized("Administrador is not authenticated.");
                }
                else
                {
                    product.Id_Administrador = userId.Value;
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

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting ID: {ex.Message}");
            }
        }

        [HttpDelete("{productId}", Name = "DeleteProduct")]
        public IActionResult Delete(int productId)
        {
            var selectedUser = _serviceContext.Set<Product>()
                   .Where(u => u.Id_Product == productId)
                    .FirstOrDefault();

            if (selectedUser != null)
            {
                _productService.DeleteProduct(productId);

                // Devolver una respuesta con un mensaje de éxito o redirigir a una página de éxito
                return Ok(new { message = "Producto eliminado exitosamente" });
            }
            else
            {
                throw new InvalidCredentialException("El usuario no está autorizado o no existe");
            }
        }

        [HttpPut("{productId}", Name = "UpdateProduct")]
        public IActionResult Put(int productId, ProductRequestModel updatedProduct)
        {
            var selectedProduct = _serviceContext.Set<Product>()
                .Where(u => u.Id_Product == productId)
                .FirstOrDefault();

            if (selectedProduct != null)
            {
                _productService.UpdateProduct(productId, updatedProduct);
                return NoContent();
            }
            else
            {
                throw new InvalidCredentialException("El producto no existe");
            }
        }

    }
}
