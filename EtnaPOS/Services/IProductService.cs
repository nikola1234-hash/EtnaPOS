using EtnaPOS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EtnaPOS.Services
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        void Delete(int id);
        Product GetProductById(int id);
        ObservableCollection<Product> GetProducts();
        void SaveData(List<Product> products);
        void UpdateProduct(int id, Product product);
    }
}