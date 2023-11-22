using Entities;

namespace emeriTEA_back_end.IServices
{
    public interface IProductService
    {
        int InsertProduct(Product product);
        void DeleteProduct(int ProductId);

        List<Product> GetProducts();
    }
}
