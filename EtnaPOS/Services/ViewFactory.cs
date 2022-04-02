using EtnaPOS.Commands;
using EtnaPOS.ViewModels;


namespace EtnaPOS.Services
{
    public class ViewFactory : IViewFactory
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly PosViewModel _posViewModel;

        public ViewFactory(HomeViewModel homeViewModel, PosViewModel posViewModel)
        {
            _homeViewModel = homeViewModel;
            _posViewModel = posViewModel;
        }

        public BaseViewModel CreateView(Navigation navigation)
        {
            switch (navigation)
            {
                case Navigation.Home:
                    return _homeViewModel;
                case Navigation.POS:
                    return _posViewModel;
                case Navigation.BackOffice:
                    return new HomeViewModel();
                default:
                    throw new System.Exception("No Views Found");
            }
        }
    }
}
