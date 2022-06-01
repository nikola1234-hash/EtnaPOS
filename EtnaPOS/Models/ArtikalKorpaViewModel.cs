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
                OnPropertyChanged();
            }
        }

        public ArtikalKorpaViewModel(Artikal artikal, int count)
        {
            Artikal = artikal;
            Count = count;
        }
    }
}
