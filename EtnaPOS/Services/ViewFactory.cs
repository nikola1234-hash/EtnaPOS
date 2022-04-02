using EtnaPOS.Commands;
using EtnaPOS.ViewModels;


namespace EtnaPOS.Services
{
    public class ViewFactory : IViewFactory
    {
        public BaseViewModel CreateView(Navigation navigation)
        {
            switch (navigation)
            {
                case Navigation.Home:
                    return new HomeViewModel();
                case Navigation.POS:
                    return new HomeViewModel();
                case Navigation.BackOffice:
                    return new HomeViewModel();
                default:
                    throw new System.Exception("No Views Found");
            }
        }
    }
}
