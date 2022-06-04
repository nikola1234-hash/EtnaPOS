using EtnaPOS.DAL.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;
using EtnaPOS.Services;

namespace EtnaPOS.ViewModels
{
    public class KasaViewModel : BaseViewModel
    {
        private int tableId { get; }
        public string TableNumber { get;}
        private EtnaDbContext db => App.GetService<EtnaDbContext>()!;
        private ObservableCollection<ArtikalKorpaViewModel> _korpa;
        public ObservableCollection<ArtikalKorpaViewModel> Korpa
        {
            get { return _korpa; }
            set 
            { 
                _korpa = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Artikal> _artikli;
        public ObservableCollection<Artikal> Artikli
        {
            get { return _artikli; }
            set
            {
                _artikli = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private bool _canDelete;

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                _canDelete = value;
                OnPropertyChanged();
            }
        }

        public ICommand DoubleClickCommand { get; }
        public ICommand RemoveCommand { get; }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value is ArtikalKorpaViewModel)
                {
                    CanDelete = true;
                }
                else
                {
                    CanDelete = false;
                }
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private decimal _totalPrice;

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }
        public KasaViewModel(int tableId)
        {
            this.tableId = tableId;
            TableNumber = "Sto: " + db.Tables.Find(tableId)!.TableName;

            DoubleClickCommand = new DelegateCommand(AddArtikalToList);
            RemoveCommand = new DelegateCommand(RemoveArtikalFromList);

            Korpa = new ObservableCollection<ArtikalKorpaViewModel>();
            InitializeArticles();

        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Korpa.Sum(x => x.TotalPrice);
        }
        protected void AddArtikalToList()
        {
            if(SelectedItem is Artikal artikal)
            {
                if (Korpa.FirstOrDefault(s => s.Artikal == artikal) != null)
                {
                    Korpa.FirstOrDefault(s => s.Artikal == artikal)!.Count += 1;
                    CalculateTotalPrice();
                }
                else
                {
                    Korpa.Add(new ArtikalKorpaViewModel(artikal, 1));
                    CalculateTotalPrice();

                }
            }
        }


        protected void RemoveArtikalFromList()
        {
            if (SelectedItem is ArtikalKorpaViewModel artikal)
            {
                if (Korpa.Contains((artikal)))
                {
                    if (Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count > 1)
                    {
                        Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count -= 1;
                        CalculateTotalPrice();
                    }
                    else if(Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count == 1)
                    {
                        Korpa.Remove(artikal);
                        CalculateTotalPrice();
                    }
                }
            }
        }
        protected void InitializeArticles()
        {
            if (Artikli == null)
            {
                Artikli = new ObservableCollection<Artikal>();
            }
            else if (Artikli.Count > 0)
            {
                Artikli.Clear();
            }
            Artikli = db.Artikli.Where(s=>s.IsActive).ToObservableCollection();
        }

        public override void Dispose()
        {
            base.Dispose();
            
        }
    }
}
