using Data;
using emeriTEA_back_end.IServices;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

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

        public void UpdateProduct(int productId, ProductRequestModel updatedProduct)
        {
            var existingProduct = _serviceContext.Product.FirstOrDefault(p => p.Id_Product == productId);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("El producto no existe.");
            }

            // Actualiza las propiedades de existingProduct con las propiedades de updatedProduct
            if (updatedProduct.Name_product != null)
            {
                existingProduct.Name_product = updatedProduct.Name_product;
            }
            if (updatedProduct.Description != null)
            {
                existingProduct.Description = updatedProduct.Description;
            }
            if (updatedProduct.Image != null)
            {
                existingProduct.Image = updatedProduct.Image;
            }
            if (updatedProduct.Size != null)
            {
                existingProduct.Size = updatedProduct.Size;
            }
            if (updatedProduct.Price != null)
            {
                existingProduct.Price = updatedProduct.Price;
            }
            if (updatedProduct.stock != null)
            {
                existingProduct.stock = updatedProduct.stock;
            }

            _serviceContext.SaveChanges();

            // Imprime solo las propiedades que deseas mostrar
            Console.WriteLine($"Name: {existingProduct.Name_product}");
            Console.WriteLine($"Description: {existingProduct.Description}");
            Console.WriteLine($"Image: {existingProduct.Image}");
            Console.WriteLine($"Size: {existingProduct.Size}");
            Console.WriteLine($"Price: {existingProduct.Price}");
            Console.WriteLine($"Stock: {existingProduct.stock}");
        }

    }
}
