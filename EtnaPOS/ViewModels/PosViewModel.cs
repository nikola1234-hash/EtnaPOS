using EtnaPOS.Models;
using System.Collections.ObjectModel;

namespace EtnaPOS.ViewModels
{
    public class PosViewModel : BaseViewModel
    {
        private bool _allowDrop;

        public bool AllowDrop
        {
            get { return _allowDrop; }
            set 
            { 
                _allowDrop = value;
                OnPropertyChanged();
                
            }
        }
        private ObservableCollection<Table> _tables;

        public ObservableCollection<Table> Tables
        {
            get { return _tables; }
            set 
            { 
                _tables = value;
                OnPropertyChanged();
            }
        }
        public PosViewModel()
       {
            Tables = new ObservableCollection<Table>();
            Tables.Add(new Table("Test"));
        }


      


    }
}
