using Entities;

namespace emeriTEA_back_end.IServices
{
    public interface IProductService
    {
        int AddProductWithSizes(Product product);
        void DeleteProduct(int ProductId);

        void UpdateProduct(int productId, ProductRequestModel updatedProduct);

        List<object> GetProducts();

    }
}

