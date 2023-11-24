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
            var existingProduct = _serviceContext.Product
                .Include(p => p.ProductSize)
                .FirstOrDefault(p => p.Id_Product == productId);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("El producto no existe.");
            }

            existingProduct.Name_product = updatedProduct.Name_product;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Image = updatedProduct.Image;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.stock = updatedProduct.stock;
            existingProduct.size = updatedProduct.size;
            existingProduct.Id_Category = updatedProduct.Id_Category;
            existingProduct.Id_Administrador = updatedProduct.Id_Administrador;

            if (existingProduct.Id_Category == 1)
            {
                if (existingProduct.ProductSize.Any())
                {
                    _serviceContext.ProductSize.RemoveRange(existingProduct.ProductSize);
                }
            }
            else if (existingProduct.Id_Category == 2)
            {

                _serviceContext.ProductSize.RemoveRange(existingProduct.ProductSize);

                if (updatedProduct.size.Any())
                {
                    var sizes = _serviceContext.Size.ToList();

                    foreach (var newSizeName in updatedProduct.size)
                    {

                        var sizeByName = sizes.FirstOrDefault(s => string.Equals(s.Name_size, newSizeName, StringComparison.OrdinalIgnoreCase));

                        if (sizeByName != null)
                        {
                            var newSizeAssociation = new ProductSize
                            {
                                ProductId = existingProduct.Id_Product,
                                SizeId = sizeByName.Id_size
                            };
                            existingProduct.ProductSize.Add(newSizeAssociation);

                        }
                    }
                }
            }
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
