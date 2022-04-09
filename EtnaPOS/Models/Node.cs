using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static ObservableCollection<Category> GetNodes()
        {
            ObservableCollection<Category> nodes = new ObservableCollection<Category>();
            nodes.Add(new Category(0, "Artikli"));
                   
            return nodes;
        }
    }
   
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
