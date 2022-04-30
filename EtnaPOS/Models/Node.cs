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
        public static ObservableCollection<Category> GetNodes()
        {
            ObservableCollection<Category> nodes;
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            if (doesExist)
            {
                var file = Directory.GetCurrentDirectory() + folder + data;
                using(StreamReader sr = new StreamReader(file))
                {
                    var json = sr.ReadToEnd();
                    var nodeData = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
                    var products = productService.GetProducts();
                    if(products == null)
                    {
                        products = new ObservableCollection<Product>();
                    }
                    var query = from node in nodeData
                                join product in products on node.Id equals product.CategoryId into cat
                                select new Category()
                                {
                                    
                                    Id = node.Id,
                                    Products = new ObservableCollection<Product>(),
                                    SubCategories = node.SubCategories,
                                    Name = node.Name,
                                    Type = node.Type
                                };
                    //Dont touch DRAGONS!
                    foreach(var q in query)
                    {
                        foreach(var product in products)
                        {
                            if(q.Id == product.CategoryId)
                            {
                                q.Products.Add(product);
                            }
                        }
                        if (q.Products.Count > 0)
                        {
                            foreach (var product in q.Products)
                            {
                                if (q.Id == product.CategoryId)
                                {
                                    continue;
                                }
                                else
                                {
                                    q.Products.Remove(product);
                                    if(q.Products.Count == 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        foreach(var s in q.SubCategories)
                        {
                            if(s.Products == null)
                            {
                                s.Products = new ObservableCollection<Product>();
                            }
                           
                            foreach(var product in products)
                            {
                                if(product.CategoryId == s.Id)
                                {
                                    s.Products.Add(product);
                                }
                            }
                        }
                    }
                    nodes = new ObservableCollection<Category>(query.ToList());
                    return nodes;
                }
               
            }
            
            return null;
        }
        public static void SaveNodes(ObservableCollection<Category> categories)
        {
            check:
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            
            if (doesExist)
            {
                var file = Directory.GetCurrentDirectory() + folder + data;
                var serialized = JsonConvert.SerializeObject(categories);
                File.WriteAllText(file, serialized);
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Data");
                goto check;
            }
           
            
            
        }

    }
    [Serializable]
    public class Category : BaseModel
    {
        public TypeNode Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        private ObservableCollection<Category> _subCategories;

        public ObservableCollection<Category> SubCategories
        {
            get { return _subCategories; }
            set
            { 
                _subCategories = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Product> _products;
        [JsonIgnore]
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            SubCategories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();
            Type = TypeNode.FolderClosed;
        }
        public Category()
        {

        }

    }
   
    public class Product
    {
        public Product(string name, double price, int categoryId)
        {
            CategoryId = categoryId;
            Name = name;
            IsActive = true;
            Price = price;
            Type = IsActive ? TypeNode.Active : TypeNode.NotActive;
        }
        public TypeNode Type { get; set; }
        public int CategoryId { get; set; }
        public int Id{ get; set; }
        public string Name { get; set; }
        public double Price { get; set; }   
        public bool IsActive { get; set; }  
     
    }
}
