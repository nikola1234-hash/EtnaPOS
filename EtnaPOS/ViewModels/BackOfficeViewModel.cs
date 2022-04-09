using DevExpress.Mvvm;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Models;
using EtnaPOS.Windows;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EtnaPOS.ViewModels
{
    public class BackOfficeViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private IEventAggregator _ea;
        public ICommand NewCategoryCommand { get; set; }
        public ICommand NewArticleCommand { get; set; }
        public ICommand DeactivateArticleCommand { get; set; }
        public ICommand ActivateArticleCommand { get; set; }
        public ICommand DeleteArticleCommand { get; set; }
        public BackOfficeViewModel()
        {
            Categories = Node.GetNodes();
            NewCategoryCommand = new DelegateCommand(CategoryWindow);
            NewArticleCommand = new DelegateCommand(CreateArticle);
            DeactivateArticleCommand = new DelegateCommand(DeactivateArticle);
            ActivateArticleCommand = new DelegateCommand(ActivateArticle);
            DeleteArticleCommand = new DelegateCommand(DeleteArticle);
            _ea = App.GetService<IEventAggregator>();
            _ea.GetEvent<PassStringEventArgs>().Subscribe(CreateNewCategory);
        }

   
        private void DeleteArticle()
        {
            throw new NotImplementedException();
        }

        private void ActivateArticle()
        {
            throw new NotImplementedException();
        }

        private void DeactivateArticle()
        {
            throw new NotImplementedException();
        }

        private void CreateArticle()
        {
            throw new NotImplementedException();
        }
        private void CategoryWindow()
        {
            var window = new CreateCategoryWindow();
            window.ShowDialog();
        }
        private void CreateNewCategory(string categoryName)
        {
            
            var artikli = Categories.FirstOrDefault(s => s.Id == 0);
            var index = artikli.SubCategories;
            int i = 0;
            if (!index.Any())
                artikli.SubCategories.Add(new Category(i, categoryName));
            else
            {
                i = index.Last().Id + 1;
                if (SelectedItem is Category category)
                {
                    category.SubCategories.Add(new Category(i, categoryName));
                }
                
                //artikli.SubCategories.Add(new Category(i, categoryName));
            }
            
           
            
        }
        public override void Dispose()
        {
            _ea.GetEvent<PassStringEventArgs>().Unsubscribe(CreateNewCategory);
            base.Dispose();
        }
    }
}
