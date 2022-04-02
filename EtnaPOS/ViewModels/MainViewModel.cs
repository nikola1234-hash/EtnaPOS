using EtnaPOS.Commands;
using EtnaPOS.Services;

namespace EtnaPOS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationStore _navigationStore;
        private readonly IViewFactory _viewFactory;

        public NavigationCommand NavigationCommand { get; }
        public MainViewModel(INavigationStore navigationStore, IViewFactory viewFactory)
        {
            _navigationStore = navigationStore;
            _viewFactory = viewFactory;
            NavigationCommand = new NavigationCommand(_navigationStore, _viewFactory);
            _navigationStore.ViewChanged += _navigationStore_ViewChanged;
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
