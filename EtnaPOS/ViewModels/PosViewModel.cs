using DevExpress.Mvvm;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.ViewModels.WindowViewModels;
using EtnaPOS.Windows;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EtnaPOS.Models;

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
        public ICommand<int> OpenTableCommand { get; set; }
        private void LoadTables()
        {
            if(Tables != null)
            {
                Tables.Clear();
            }
            var tables = db.Tables.ToList();
            if(tables != null)
            {
                Tables = new ObservableCollection<Table>();
                foreach (var table in tables)
                {
                    Tables.Add(new Table(table));
                }
            }
            OnPropertyChanged(nameof(Tables));
        }
        public PosViewModel(IEventAggregator ea)
        {
            LoadTables();

            OpenTableCommand = new DelegateCommand<int>(OpenTableWindow);
            NewTableCommand = new DelegateCommand(CreateNewTable);

            _ea = ea;
            _ea.GetEvent<ManageTableKey>().Subscribe(ManageTables);
            _ea.GetEvent<PassObjectEvent>().Subscribe(AddNewTable);
            
        }

        private void OpenTableWindow(int id)
        {
            if(_tables.Any(s=> s.Id == id))
            {
                var window = new KasaWindow()
                {
                    DataContext = new KasaViewModel(id)
                };
                window.ShowDialog();
                LoadTables();
            }
        }

        private void AddNewTable(object obj)
        {
            if (obj is EtnaPOS.DAL.Models.Table table && table != null)
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
