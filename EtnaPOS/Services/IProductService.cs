using EtnaPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EtnaPOS.Services
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        void Delete(Guid id);
        Product GetProductById(Guid id);
        ObservableCollection<Product> GetProducts();
        void SaveData(List<Product> products);
        void UpdateProduct(Guid id, Product product);
    }
}