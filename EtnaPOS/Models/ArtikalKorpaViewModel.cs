using EtnaPOS.DAL.Models;
using EtnaPOS.ViewModels;

namespace EtnaPOS.Models
{
    public class ArtikalKorpaViewModel : BaseViewModel
    {
        public Artikal Artikal { get; set; }
        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {

                _count = value;
                TotalPrice = Count * Artikal.Price;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public decimal TotalPrice { get; set; }
        public ArtikalKorpaViewModel(Artikal artikal, int count)
        {
            Artikal = artikal;
            Count = count;
        }
    }
}
