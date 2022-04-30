using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

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
        private static string folder => "\\Data";
        private static string data => "\\treedata.json";
        public static ObservableCollection<Category> GetNodes()
        {
            var doesExist = Directory.Exists(Directory.GetCurrentDirectory() + folder);
            if (doesExist)
            {
                var file = Directory.GetCurrentDirectory() + folder + data;
                using(StreamReader sr = new StreamReader(file))
                {
                    var json = sr.ReadToEnd();
                    var nodes = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
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

    }
   
    public class Product
    {
        public Product(int id,string name, double price, int categoryId)
        {
            Id = id;
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
