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
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
            }
            return products;
        }
        public Product GetProductById(Guid id)
        {
            var file = fullPath;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                var product = JsonConvert.DeserializeObject<List<Product>>(json).FirstOrDefault(s => s.Id == id);
                return product;
            }
        }
        public Product AddProduct(Product product)
        {
            var file = fullPath;
            List<Product> products;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
                if (products == null)
                {
                    products = new List<Product>();
                }
                
                products.Add(product);

            }
            SaveData(products);
            return product;
        }
        public void UpdateProduct(Guid id, Product product)
        {
            var file = fullPath;
            List<Product> products;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
                foreach (var item in products)
                {
                    if (item.Id == id)
                    {
                        item.Price = product.Price;
                        item.Name = product.Name;
                        item.IsActive = product.IsActive;
                        
                    }
                }
            }
            if(products!= null)
            {
                SaveData(products);
            }
        }
        public void Delete(Guid id)
        {
            var file = fullPath;
            List<Product> products;
            using (StreamReader sr = new StreamReader(file))
            {
                var json = sr.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
                foreach (var item in products)
                {
                    if (item.Id == id)
                    {
                        products.Remove(item);
                        
                    }
                }
            }
            if (products != null)
            {
                SaveData(products);
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
