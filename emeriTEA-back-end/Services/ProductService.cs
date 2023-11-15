using Data;
using emeriTEA_back_end.IServices;
using Entities;

namespace emeriTEA_back_end.Services
{
    public class ProductService : BaseContextService, IProductService
    {
        public ProductService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public int InsertProduct(Product product)
        {
            _serviceContext.Product.Add(product);
            _serviceContext.SaveChanges();
            return product.Id_Product;
        }



    }
}
