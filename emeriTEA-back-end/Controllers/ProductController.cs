using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Data;
using emeriTEA_back_end.IServices;
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

        [HttpPost]
        public IActionResult AddProductWithSizes([FromBody] Product product)
        {

            //var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
            var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);

            if (userId == null)
            {

                return Unauthorized("User is not authenticated.");
            }
            else
            {
                product.Id_Administrador = (int)userId;
                var productId = _productService.AddProductWithSizes(product);
                if (productId != -1)
                {
                    return Ok(productId);
                }
                return BadRequest("Algunas tallas no fueron encontradas");
            }
        }



        [HttpDelete("{productId}", Name = "DeleteProduct")]
        public IActionResult Delete(int productId)
        {

            //var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
            var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);

            if (userId == null)
            {

                return Unauthorized("User is not authenticated.");
            }
            else
            {
                var selectedUser = _serviceContext.Set<Product>()
                       .Where(u => u.Id_Product == productId)
                        .FirstOrDefault();

                if (selectedUser != null)
                {
                    _productService.DeleteProduct(productId);

                    return Ok(new { message = "Producto eliminado exitosamente" });
                }
                else
                {
                    throw new InvalidCredentialException("El usuario no está autorizado o no existe");
                }
            }

        }

        [HttpPut("{productId}", Name = "UpdateProduct")]
        public IActionResult Put( int productId, [FromBody] Product updatedProduct)
        {
            var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
            //var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);

            if (userId == null)
            {

                return Unauthorized("User is not authenticated.");
            }
            else
            {
                updatedProduct.Id_Administrador = (int)userId;
                _productService.UpdateProduct(productId, updatedProduct);
                return Ok(new { message = "Producto editado exitosamente" });
            }
        }

        [HttpGet(Name = "GetProducts")]
        public List<object> GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpGet(Name = "GetProductsByCategory")]
        public List<object> GetProductsByCategory(int categotyId)
        {
            return _productService.GetProductsByCategory(categotyId);
        }




    }
}
