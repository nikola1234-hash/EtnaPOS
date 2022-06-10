using EtnaPOS.Commands;
using EtnaPOS.ViewModels;


namespace EtnaPOS.Services
{
    public class ViewFactory : IViewFactory
    {
        private readonly HomeViewModel _homeViewModel;

        public ViewFactory(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;

        }

        public BaseViewModel CreateView(Navigation navigation)
        {
            switch (navigation)
            {
                case Navigation.Home:
                    return _homeViewModel;
                case Navigation.POS:
                    return new PosViewModel();
                case Navigation.BackOffice:
                    return new HomeViewModel();
                default:
                    throw new System.Exception("No Views Found");
            }
        }
    }
}
