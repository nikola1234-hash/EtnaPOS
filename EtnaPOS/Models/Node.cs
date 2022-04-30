using EtnaPOS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace EtnaPOS.Models
{
    public enum TypeNode
    {
        Active,
        NotActive,
        FolderOpen,
        FolderClosed
    }
    
    public class Node
    {
        private static IProductService productService = App.GetService<IProductService>();
        private static string folder => "\\Data";
        private static string data => "\\treedata.json";
        public static ObservableCollection<Product> GetNodes()
        {
            ObservableCollection<Product> nodes;
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            if (doesExist)
            {
                var file = Directory.GetCurrentDirectory() + folder + data;
                ObservableCollection<Product> nodeData;
                using (StreamReader sr = new StreamReader(file))
                {
                    var json = sr.ReadToEnd();
                    nodeData = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);      
                }
                if (nodeData == null)
                {
                    nodeData = new ObservableCollection<Product>();
                    nodeData.Add(new Product(Guid.NewGuid(), "Artikli"));
                    SaveNodes(nodeData);
                }
                return nodeData;
            }
            return null;
        }
        public static void SaveNodes(ObservableCollection<Product> products)
        {
            check:
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            
            if (doesExist)
            {
                var file = Directory.GetCurrentDirectory() + folder + data;
                var serialized = JsonConvert.SerializeObject(products);
                File.WriteAllText(file, serialized);
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Data");
                goto check;
            }
           
            
            
        }

    }
   
    public class Product : BaseModel
    {
        public Product()
        {

        }
        public Product(Guid id, string name)
        {
            Id = id;
            Name = name;
            Type = TypeNode.FolderClosed;
            Products = new ObservableCollection<Product>();
        }
        public Product(string name, double price)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsActive = true;
            Price = price;

            Type = IsActive ? TypeNode.Active : TypeNode.NotActive;
            
            
        }
        public TypeNode Type { get; set; }
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set 
            {
                products = value;
                OnPropertyChanged();
            }
        }

        public Guid Id{ get; set; }
        public string Name { get; set; }
        public double Price { get; set; }   
        public bool IsActive { get; set; }  
     
    }
}
