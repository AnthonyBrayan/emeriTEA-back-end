    using Data;
using emeriTEA_back_end.IServices;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace emeriTEA_back_end.Services
{
    public class ProductService : BaseContextService, IProductService
    {
        public ProductService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

         public int AddProductWithSizes(Product product)

        {
            if (product.size != null && product.size.Length > 0)
            {
                var sizes = new List<Size>();

                foreach (var sizeName in product.size)
                {
                    var size = _serviceContext.Size.FirstOrDefault(s => s.Name_size == sizeName);
                    if (size != null)
                    {
                        sizes.Add(size);
                    }
                }

                if (sizes.Count != product.size.Length)
                {
                    return -1;
                }

                _serviceContext.Product.Add(product);
                _serviceContext.SaveChanges();

                foreach (var size in sizes)
                {
                    _serviceContext.ProductSize.Add(new ProductSize { Product = product, Size = size });
                }

                _serviceContext.SaveChanges();
                return product.Id_Product;
            }
            else
            {
                _serviceContext.Product.Add(product);
                _serviceContext.SaveChanges();
                return product.Id_Product;
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _serviceContext.Product.Find(productId);
            if (product != null)
            {
                _serviceContext.Product.Remove(product);
                _serviceContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("El producto no existe.");
            }
        }

        public List<object> GetProducts()
        {
            var productsWithProductSizes = _serviceContext.Product
                .Select(p => new
                {
                    p.Id_Product,
                    p.Name_product,
                    p.Description,
                    p.Image,
                    size = p.ProductSize.Select(ps => ps.Size.Name_size).ToList(),
                    p.Price,
                    p.stock,
                    p.Id_Category
                })
                .ToList<object>();

            return productsWithProductSizes;
        }
    }
}
