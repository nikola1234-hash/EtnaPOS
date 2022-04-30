using DevExpress.Mvvm;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using EtnaPOS.Services;
using EtnaPOS.ViewModels.WindowViewModels;
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
        private IProductService productService => App.GetService<IProductService>();    
        public ObservableCollection<Product> Products { get; set; }
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
            Products = Node.GetNodes();
            NewCategoryCommand = new DelegateCommand(CategoryWindow);
            NewArticleCommand = new DelegateCommand(CreateArticleWindow);
            DeactivateArticleCommand = new DelegateCommand(DeactivateArticle);
            ActivateArticleCommand = new DelegateCommand(ActivateArticle);
            DeleteArticleCommand = new DelegateCommand(DeleteArticle);
            _ea = App.GetService<IEventAggregator>();
            _ea.GetEvent<PassStringEventArgs>().Subscribe(CreateNewCategory);
            _ea.GetEvent<PassNewProductEventArgs>().Subscribe(CreateArticle);
        }

        private void CreateArticleWindow()
        {
            if(SelectedItem is Product product)
            {
                CreateProductWindow window = new CreateProductWindow
                {
                    DataContext = new CreateProductViewModel()
                };
                window.ShowDialog();
            }
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

        private void CreateArticle(object obj)
        {
            
            if (SelectedItem is Product pr)
            {
                if (obj is Product product)
                {
                    var newProduct = productService.AddProduct(product);
                    pr.Products.Add(newProduct);
                    Node.SaveNodes(Products);
                }
            }
            
        }
        private void CategoryWindow()
        {
            var window = new CreateCategoryWindow();
            window.ShowDialog();
        }
        private void CreateNewCategory(string categoryName)
        {
            
            
        if (SelectedItem is Product product)
        {
            product.Products.Add(new Product(Guid.NewGuid(), categoryName));
                    
        }

            Node.SaveNodes(Products);
        }
        public override void Dispose()
        {
            _ea.GetEvent<PassStringEventArgs>().Unsubscribe(CreateNewCategory);
            _ea.GetEvent<PassNewProductEventArgs>().Unsubscribe(CreateArticle);
            base.Dispose();
        }
    }
}
