using EtnaPOS.ViewModels;
using System;
namespace EtnaPOS.Services
{
    public class NavigationStore : INavigationStore
    {
        public event Action ViewChanged;
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnViewChange();
            }
        }

        private void OnViewChange()
        {
            ViewChanged?.Invoke();
        }
    }
}
