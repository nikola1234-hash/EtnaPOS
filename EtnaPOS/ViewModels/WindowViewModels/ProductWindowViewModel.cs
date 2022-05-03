using DevExpress.Mvvm;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using Prism.Events;
using System;
using System.Windows.Input;

namespace EtnaPOS.ViewModels.WindowViewModels
{
    public class ProductWindowViewModel : BaseViewModel
    {
        private IEventAggregator eventAggregator => App.GetService<IEventAggregator>();
        private ICurrentWindowService WindowService => GetService<ICurrentWindowService>();
        private string _name;
        public int Id { get; set; }
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
        public int KategorijaId { get; set; }
        public double Price
        {
            get { return _price; }
            set 
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public ICommand CreateCommand { get; set; }
        public ProductWindowViewModel(Artikal artikal)
        {
            Id = artikal.Id;
            Name = artikal.Name;
            Price = artikal.Price;
            KategorijaId = artikal.KategorijaArtiklaId;
            CreateCommand = new DelegateCommand(SaveEditedProduct);
        }

        private void SaveEditedProduct()
        {
            var artikal = new Artikal
            {
                Id = Id,
                Name = Name,
                Price = Price,
                KategorijaArtiklaId = KategorijaId
            };
            eventAggregator.GetEvent<EditedProductEventArgs>().Publish(artikal);
            WindowService.Close();
        }

        public ProductWindowViewModel(int kategorijaId)
        {
            KategorijaId = kategorijaId;
            CreateCommand = new DelegateCommand(CreateProduct);
        }
        public void CreateProduct()
        {
            Artikal artikal = new Artikal(Name, Price, KategorijaId);
            eventAggregator.GetEvent<PassNewProductEventArgs>().Publish(artikal);
            WindowService.Close();
        }
    }
}
