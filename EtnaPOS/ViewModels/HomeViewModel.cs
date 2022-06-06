using System.Windows.Input;
using EtnaPOS.Windows;
using Prism.Commands;

namespace EtnaPOS.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand OpenDesignerCommand { get; }
        public HomeViewModel()
        {
            OpenDesignerCommand = new DelegateCommand(OpenDesigner);
        }

        private void OpenDesigner()
        {
            ReportDesigner repoeDesigner = new ReportDesigner();
            repoeDesigner.ShowDialog();
        }
    }
}
