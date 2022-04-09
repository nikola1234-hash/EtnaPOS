using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

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
        
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        
        public static List<Category> GetNodes()
        {
            List<Category> nodes = new List<Category>();
            nodes.Add(new Category(0, "Pice"));
            nodes.Add(new Category(1, "Hrana"));
            var t = nodes.FirstOrDefault(i => i.Id == 0);
            t.SubCategories.Add(new Category(0, "Sokovi"));
            t.SubCategories.FirstOrDefault(s => s.Id == 0)?.Products.Add(new Product(0, "Coca Cola", 155.00));
            t.SubCategories.FirstOrDefault(s => s.Id == 0)?.Products.Add(new Product(1, "Fanta", 155.00));
            t.SubCategories.FirstOrDefault(s => s.Id == 0)?.Products.Add(new Product(1, "Fanta", 155.00));
            return nodes;
        }
    }
   
    public class Category
    {
        public TypeNode Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<Product> Products { get; set; }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            SubCategories = new List<Category>();
            Products = new List<Product>();
            Type = TypeNode.FolderClosed;
        }
    }
   
    public class Product
    {


        public Product(int id,string name, double price)
        {
            Id = id;

            Name = name;
            IsActive = true;
            Price = price;
            Type = IsActive ? TypeNode.Active : TypeNode.NotActive;
        }
        public TypeNode Type { get; set; }
        public int Id{ get; set; }
        public string Name { get; set; }
        public double Price { get; set; }   
        public bool IsActive { get; set; }  
     
    }
}
