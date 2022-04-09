using DevExpress.Mvvm;
using EtnaPOS.Commands;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Services;
using EtnaPOS.Windows;
using Prism.Events;
using System;
using System.Windows.Input;

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
        public MainViewModel(INavigationStore navigationStore, IViewFactory viewFactory, IEventAggregator ea)
        {
            _navigationStore = navigationStore;
            _viewFactory = viewFactory;
            NavigationCommand = new NavigationCommand(_navigationStore, _viewFactory);
            _navigationStore.ViewChanged += _navigationStore_ViewChanged;
            ManageTablesCommand = new DelegateCommand(OnKeyCombinationPresses);
            BackofficeCommand = new DelegateCommand(OpenBackofficeWindow);
            _ea = ea;   
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
