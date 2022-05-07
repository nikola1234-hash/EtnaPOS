using DevExpress.Mvvm;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private decimal _price;
        public int KategorijaId { get; set; }
        public List<KategorijaArtikla> ListaKategorija { get; set; }
        private KategorijaArtikla selectedItem;
        public KategorijaArtikla SelectedItem { 
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }
        public decimal Price
        {
            get { return _price; }
            set 
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public bool ShowDropbox { get; set; }
        public ICommand CreateCommand { get; set; }
        public ProductWindowViewModel(Artikal artikal, IEnumerable<KategorijaArtikla> listaKategorija)
        {
            Id = artikal.Id;
            Name = artikal.Name;
            Price = artikal.Price;
            KategorijaId = artikal.KategorijaArtiklaId;
            CreateCommand = new DelegateCommand(SaveEditedProduct);
            ListaKategorija = listaKategorija.ToList();
            ShowDropbox = ListaKategorija.Count > 0;
        }

        private void SaveEditedProduct()
        {
            var artikal = new Artikal
            {
                Id = Id,
                Name = Name,
                Price = (Decimal)Price,
                KategorijaArtiklaId = SelectedItem.Id
            };
            eventAggregator.GetEvent<EditedProductEventArgs>().Publish(artikal);
            WindowService.Close();
        }

        public ProductWindowViewModel(int kategorijaId)
        {
            KategorijaId = kategorijaId;
            CreateCommand = new DelegateCommand(CreateProduct);
            ShowDropbox = false;
        }
        public void CreateProduct()
        {
            Artikal artikal = new Artikal
            {
                Name = Name,
                Price = (Decimal)Price,
                DateCreated = DateTime.Now,
                CreatedBy = "System",
                KategorijaArtiklaId = KategorijaId,
                IsActive = true
            };
            eventAggregator.GetEvent<PassNewProductEventArgs>().Publish(artikal);
            WindowService.Close();
        }
    }
}
