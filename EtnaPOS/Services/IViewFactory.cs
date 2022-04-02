using EtnaPOS.Commands;
using EtnaPOS.ViewModels;

namespace EtnaPOS.Services
{
    public interface IViewFactory
    {
        BaseViewModel CreateView(Navigation navigation);
    }
}