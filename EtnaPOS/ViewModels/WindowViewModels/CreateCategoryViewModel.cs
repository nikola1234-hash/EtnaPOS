using DevExpress.Mvvm;
using EtnaPOS.EtnaEventArgs;
using Prism.Events;

using System.Windows.Input;

namespace EtnaPOS.ViewModels.WindowViewModels
{
    public class CreateCategoryViewModel : BaseViewModel
    {

        public ICommand SubmitCommand => new DelegateCommand(OnSubmit);
        private ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
        private IEventAggregator _ea => App.GetService<IEventAggregator>();

        private string _categoryName;

        public string CategoryName
        {
            get { return _categoryName; }
            set
            { 
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        private void OnSubmit()
        {
            _ea.GetEvent<PassStringEventArgs>().Publish(CategoryName);
            CurrentWindowService.Close();
        }
    }
}
