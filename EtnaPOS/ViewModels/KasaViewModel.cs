using EtnaPOS.DAL.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;

namespace EtnaPOS.ViewModels
{
    public class KasaViewModel : BaseViewModel
    {
        private int tableId { get; }
        private EtnaDbContext db => App.GetService<EtnaDbContext>();

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
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public KasaViewModel(int tableId)
        {
            this.tableId = tableId;

            RemoveCommand = new DelegateCommand(RemoveArtikalFromList);
            AddCommand = new DelegateCommand(AddArtikalToList);

            Korpa = new ObservableCollection<ArtikalKorpaViewModel>();

            InitializeArticles();

        }

        protected void AddArtikalToList()
        {
            if(SelectedItem != null && SelectedItem is Artikal artikal)
            {
                if (Korpa.FirstOrDefault(s => s.Artikal == artikal) != null)
                {
                    Korpa.FirstOrDefault(s => s.Artikal == artikal)!.Count += 1;
                }
                else
                {
                    Korpa.Add(new ArtikalKorpaViewModel(artikal, 0));
                }
            }
        }

        protected void RemoveArtikalFromList()
        {
            if (SelectedItem != null && SelectedItem is ArtikalKorpaViewModel artikal)
            {
                if (Korpa.Contains((artikal)))
                {
                    if (Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count > 1)
                    {
                        Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count -= 1;
                    }
                    else if(Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count == 1)
                    {
                        Korpa.Remove(artikal);
                    }
                }
            }
        }
        private void InitializeArticles()
        {
            if (Artikli == null)
            {
                Artikli = new ObservableCollection<Artikal>();
            }
            else if (Artikli.Count > 0)
            {
                Artikli.Clear();
            }
            Artikli = db.Artikli.ToObservableCollection();
        }
    }
}
