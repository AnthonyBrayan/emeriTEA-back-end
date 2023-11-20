using Entities;

namespace emeriTEA_back_end.IServices
{
    public interface IProductService
    {
        int InsertProduct(Product product);
        void DeleteProduct(int ProductId);
        void UpdateProduct(int productId, ProductRequestModel updatedProduct);
    }
}

