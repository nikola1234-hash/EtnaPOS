using DevExpress.Mvvm;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using Prism.Events;
using System.Windows.Input;

namespace EtnaPOS.ViewModels.WindowViewModels
{
    public class AddTableViewModel : BaseViewModel
    {
        public string Label => "Naziv stola: ";
        private string categoryName;
        private IEventAggregator ea => App.GetService<IEventAggregator>();
        private ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
        public string CategoryName
        {
            get { return categoryName; }
            set 
            {
                categoryName = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; set; }
        public AddTableViewModel()
        {
            SubmitCommand = new DelegateCommand(CreateTable);
        }

        private void CreateTable()
        {
            Table table = new Table()
            {
                TableName = CategoryName
            };
            ea.GetEvent<PassObjectEvent>().Publish(table);
            CurrentWindowService.Close();
        }
    }
}
