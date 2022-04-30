using DevExpress.Mvvm;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using Prism.Events;
using System.Windows.Input;

namespace EtnaPOS.ViewModels.WindowViewModels
{
    public class CreateProductViewModel : BaseViewModel
    {
        private IEventAggregator eventAggregator => App.GetService<IEventAggregator>();
        private ICurrentWindowService WindowService => GetService<ICurrentWindowService>();
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {          
                _name = value;
                OnPropertyChanged();
            }
        }
        private double _price;

        public double Price
        {
            get { return _price; }
            set 
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public int CategoryId { get; set; }
        public ICommand CreateCommand { get; set; }
        public CreateProductViewModel()
        {
            CreateCommand = new DelegateCommand(CreateProduct);
        }
        public void CreateProduct()
        {
            Product product = new Product(Name, Price);
            eventAggregator.GetEvent<PassNewProductEventArgs>().Publish(product);
            WindowService.Close();
        }
    }
}
