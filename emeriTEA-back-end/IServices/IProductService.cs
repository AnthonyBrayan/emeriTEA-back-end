using Entities;

namespace emeriTEA_back_end.IServices
{
    public interface IProductService
    {
        int AddProductWithSizes(Product product);
        void DeleteProduct(int ProductId);
        void UpdateProduct(int productId, Product updatedProduct);
        List<object> GetProducts();
        List<object> GetProductsByCategory(int categoryId);


    }
}
