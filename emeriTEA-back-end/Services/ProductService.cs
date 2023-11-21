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

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            var existingProduct = _serviceContext.Product.FirstOrDefault(p => p.Id_Product == productId);

            if (existingProduct == null)
            {
                // Si el producto no existe, podrías lanzar una excepción o manejar el caso según tus requerimientos.
                throw new InvalidOperationException("El producto no existe.");
            }

            // Actualiza las propiedades del producto con la información del producto modificado
            existingProduct.Name_product = updatedProduct.Name_product;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Image = updatedProduct.Image;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.stock = updatedProduct.stock;
            existingProduct.size = updatedProduct.size;
            existingProduct.Id_Category = updatedProduct.Id_Category;
            existingProduct.Id_Administrador = updatedProduct.Id_Administrador;

            _serviceContext.SaveChanges();
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

        public List<object> GetProductsByCategory(int categoryId)
        {
            var productsByCategory = _serviceContext.Product
                .Where(p => p.Id_Category == categoryId)
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

            return productsByCategory;
        }


    }
}
