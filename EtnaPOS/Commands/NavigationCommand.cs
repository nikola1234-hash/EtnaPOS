using EtnaPOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EtnaPOS.Commands
{
    public class NavigationCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly INavigationStore _navigationStore;
        private readonly IViewFactory _viewFactory;


        public NavigationCommand(INavigationStore navigationStore, IViewFactory viewFactory)
        {
            _navigationStore = navigationStore;
            _viewFactory = viewFactory;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter is Navigation navigation)
            {
                _navigationStore.CurrentViewModel = _viewFactory.CreateView(navigation);
            }
        }
    }
}
