using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EtnaPOS.ViewModels
{
    public class PosViewModel : BaseViewModel
    {
        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value; 
                
                OnPropertyChanged();
            }
        }

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
        private readonly IEventAggregator _ea;
        public ICommand NewTableCommand { get; set; }
        
        public PosViewModel(IEventAggregator ea)
        {
            Tables = new ObservableCollection<Table>(); 
            Tables.Add(new Table("Hello World"));
            NewTableCommand = new DelegateCommand(CreateNewTable);
            _ea = ea;
            _ea.GetEvent<ManageTableKey>().Subscribe(ManageTables);
            
        }

        private void CreateNewTable()
        {
           
        }

        private void ManageTables()
        {
            IsVisible = !IsVisible;
            OnPropertyChanged(nameof(IsVisible));
            AllowDrop = !AllowDrop;
            OnPropertyChanged(nameof(AllowDrop));
        }
        public override void Dispose()
        {
            _ea.GetEvent<ManageTableKey>().Unsubscribe(ManageTables);
            base.Dispose();
        }
    }
}
