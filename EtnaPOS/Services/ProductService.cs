using EtnaPOS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtnaPOS.Services
{
    public class ProductService : IProductService
    {
        private static string folder => "\\Data";
        private static string data => "\\products.json";
        private static string fullPath => Directory.GetCurrentDirectory() + folder + data;

        public ObservableCollection<Product> GetProducts()
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                return products;
            }
        }
        public Product GetProductById(int id)
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var product = JsonConvert.DeserializeObject<List<Product>>(json).FirstOrDefault(s => s.Id == id);
                return product;
            }
        }
        public void CreateProduct(Product product)
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                var productId = products.Last().Id;
                product.Id = productId + 1;
                products.Add(product);
                SaveData(products);
            }
        }
        public void UpdateProduct(int id, Product product)
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                foreach (var item in products)
                {
                    if (item.Id == id)
                    {
                        item.CategoryId = product.CategoryId;
                        item.Price = product.Price;
                        item.Name = product.Name;
                        item.IsActive = product.IsActive;
                        SaveData(products);
                    }
                }
            }
        }
        public void Delete(int id)
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var products = JsonConvert.DeserializeObject<List<Product>>(json);
                foreach (var item in products)
                {
                    if (item.Id == id)
                    {
                        products.Remove(item);
                        SaveData(products);
                    }
                }
            }
        }
        public void SaveData(List<Product> products)
        {
            var file = fullPath;
            var serialized = JsonConvert.SerializeObject(products);
            File.WriteAllText(file, serialized);
        }
    }
}
