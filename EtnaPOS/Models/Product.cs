using System;
using System.Collections.ObjectModel;

namespace EtnaPOS.Models
{
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

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }

    }
}
