using EtnaPOS.ViewModels;
using System;

namespace EtnaPOS.Services
{
    public interface INavigationStore
    {
        BaseViewModel CurrentViewModel { get; set; }

        event Action ViewChanged;
    }
}