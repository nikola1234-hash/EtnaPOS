using DevExpress.Mvvm;
using EtnaPOS.Commands;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Services;
using EtnaPOS.Windows;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace EtnaPOS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationStore _navigationStore;
        private readonly IViewFactory _viewFactory;
        private readonly IEventAggregator _ea;
        public ICommand ManageTablesCommand { get; set; }
        public NavigationCommand NavigationCommand { get; }
        public ICommand BackofficeCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        private DateTime _selectedDate = DateTime.Now.Date;
        protected IDialogService DialogService
        {
            get;
            set;
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        protected void ShowDialog()
        {
            DialogService = this.GetService<IDialogService>("ChoseDate");
            var dsViewModel = new DialogServiceViewModel();
            var result = DialogService.ShowDialog(dsViewModel.DialogCommands, "Radni dan", viewModel: dsViewModel);
            if ((MessageBoxResult)result.Id == MessageBoxResult.OK)
            {
                return;
            }
            else
            {
                MessageBox.Show("Morate izabrati radni dan");
                Environment.Exit(Environment.ExitCode);
            }
        }
        public MainViewModel(INavigationStore navigationStore, IViewFactory viewFactory, IEventAggregator ea)
        {
            _navigationStore = navigationStore;
            _viewFactory = viewFactory;
            NavigationCommand = new NavigationCommand(_navigationStore, _viewFactory);
            _navigationStore.ViewChanged += _navigationStore_ViewChanged;
            ManageTablesCommand = new DelegateCommand(OnKeyCombinationPresses);
            BackofficeCommand = new DelegateCommand(OpenBackofficeWindow);
            LoadedCommand = new DelegateCommand(OnLoaded);
            _ea = ea;
          
        }

        private void OnLoaded()
        {
            ShowDialog();
        }

        private void OpenBackofficeWindow()
        {
            var window = new BackOfficeWindow();
            window.ShowDialog();
        }

        private void OnKeyCombinationPresses()
        {
            _ea.GetEvent<ManageTableKey>().Publish();
        }

        private void _navigationStore_ViewChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public override void Dispose()
        {
            _navigationStore.ViewChanged -= _navigationStore_ViewChanged;
            CurrentViewModel.Dispose();
            base.Dispose();
        }
    }
}
