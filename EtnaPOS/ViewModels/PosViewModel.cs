using DevExpress.Mvvm;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.ViewModels.WindowViewModels;
using EtnaPOS.Windows;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EtnaPOS.ViewModels
{
    public class PosViewModel : BaseViewModel
    {
        private bool _isVisible;
        private EtnaDbContext db => App.GetService<EtnaDbContext>();
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value; 
                
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
        private readonly IEventAggregator _ea;
        public ICommand NewTableCommand { get; set; }
        private void LoadTables()
        {
            if(Tables != null)
            {
                Tables.Clear();
            }
            var tables = db.Tables;
            if(tables != null)
            {
                Tables = new ObservableCollection<Table>(tables);
            }
            OnPropertyChanged(nameof(Tables));
        }
        public PosViewModel(IEventAggregator ea)
        {
            LoadTables();

            NewTableCommand = new DelegateCommand(CreateNewTable);
            _ea = ea;
            _ea.GetEvent<ManageTableKey>().Subscribe(ManageTables);
            _ea.GetEvent<PassObjectEvent>().Subscribe(AddNewTable);
            
        }

     

        private void AddNewTable(object obj)
        {
            if (obj is Table table && table != null)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                LoadTables();
            }
            
        }

        private void CreateNewTable()
        {
            // Using this as generic single add item window
            CreateCategoryWindow window = new CreateCategoryWindow
            {
                DataContext = new AddTableViewModel()
            };
            window.ShowDialog();
        }

        private void ManageTables()
        {
            IsVisible = !IsVisible;
            OnPropertyChanged(nameof(IsVisible));
        }
        public override void Dispose()
        {
            _ea.GetEvent<PassObjectEvent>().Unsubscribe(AddNewTable);
            _ea.GetEvent<ManageTableKey>().Unsubscribe(ManageTables);
            base.Dispose();
        }
    }
}
