using DevExpress.Mvvm;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using EtnaPOS.Models;
using EtnaPOS.Services;
using EtnaPOS.ViewModels.WindowViewModels;
using EtnaPOS.Windows;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EtnaPOS.ViewModels
{
    public class BackOfficeViewModel : BaseViewModel
    {
        private ITreeViewNodeGenerator _treeViewNodeGenerator => App.GetService<ITreeViewNodeGenerator>();
        public ObservableCollection<Node> TreeNodes { get; set; }
        private object _selectedItem;
        private EtnaDbContext _db => App.GetService<EtnaDbContext>();
        public string AddArticleText
        {
            get 
            {
                if(SelectedItem is Product product && product.Type != TypeNode.FolderClosed)
                {
                    return "Izmeni";
                    
                }
                else
                {
                    return "Novi artikal";
                }
                
            }
        }

        private ILogger<BackOfficeViewModel> logger => App.GetService<ILogger<BackOfficeViewModel>>();  
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
            CreateInitialCategory();
            GenerateNodes();
            NewCategoryCommand = new DelegateCommand(CategoryWindow);
            NewArticleCommand = new DelegateCommand(CreateArticleWindow);
            DeactivateArticleCommand = new DelegateCommand(DeactivateArticle);
            ActivateArticleCommand = new DelegateCommand(ActivateArticle);
            DeleteArticleCommand = new DelegateCommand(DeleteArticle);
            _ea = App.GetService<IEventAggregator>();
            _ea.GetEvent<PassStringEventArgs>().Subscribe(CreateNewCategory);
            _ea.GetEvent<PassNewProductEventArgs>().Subscribe(CreateArticle);
            _ea.GetEvent<EditedProductEventArgs>().Subscribe(EditArticle);
            logger.LogInformation("Backoffice logger test.");
        }
        private void GenerateNodes()
        {
            if(TreeNodes == null)
            {
                TreeNodes = _treeViewNodeGenerator.GenerateNodes();
            }
            else
            {
                TreeNodes.Clear();
                TreeNodes = _treeViewNodeGenerator.GenerateNodes();
            }
            OnPropertyChanged(nameof(TreeNodes));
        }
        private void DeleteArticle()
        {
            throw new NotImplementedException();
        }

        private void EditArticle(object obj)
        {
            var item = (Artikal)obj;
            try
            {
                var toUpdate = _db.Artikli.FirstOrDefault(s => s.Id == item.Id);
                if(toUpdate != null)
                {
                    toUpdate.Price = item.Price;
                    toUpdate.Name = item.Name;
                    _db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                logger.LogError("Edit article threw an exception. Ex. Message: " + ex.Message, ex);
            }
            GenerateNodes();
        }
     
        
        private void CreateArticleWindow()
        {
            if(SelectedItem is Node node)
            {
                if(node.Type == TypeNode.FolderClosed)
                {
                    CreateProductWindow window = new CreateProductWindow
                    {
                        DataContext = new ProductWindowViewModel(node.Id)
                    };
                    window.ShowDialog();
                }
                else if(node.Type == TypeNode.Active || node.Type == TypeNode.NotActive)
                {
                    var artikal = _db.Artikli.FirstOrDefault(s => s.Id == node.Id);
                    if (artikal is null) return;
                    CreateProductWindow window = new CreateProductWindow
                    {
                        DataContext = new ProductWindowViewModel(artikal)
                    };
                    window.ShowDialog();
                   
                   
                }
                GenerateNodes();
             
            }
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

            if (obj is Artikal artikal)
            {
                _db.Artikli.Add(artikal);
                _db.SaveChanges();
                GenerateNodes();
            }
            
            
        }
        private void CreateInitialCategory()
        {
            var artikli = _db.Kategorije.FirstOrDefault(s => s.Kategorija == "Artikli");
            if(artikli == null)
            {
                KategorijaArtikla kategorija = new KategorijaArtikla
                {
                    Kategorija = "Artikli",
                    DateCreated = DateTime.Now,
                    CreatedBy = "System"
                };
                _db.Kategorije.Add(kategorija);
                _db.SaveChanges();
                GenerateNodes();
            }
        }
        private void CategoryWindow()
        {
            var window = new CreateCategoryWindow();
            window.ShowDialog();
        }
        private void CreateNewCategory(string categoryName)
        {
            var kat = _db.Kategorije.SingleOrDefault(s => s.Kategorija == categoryName);
            if(kat == null)
            {
                KategorijaArtikla kategorija = new KategorijaArtikla
                {
                    Kategorija = categoryName,
                    DateCreated = DateTime.Now,
                    CreatedBy = "System",
                    ParentId = ((Node)SelectedItem).Id

                };
                _db.Kategorije.Add(kategorija);
                _db.SaveChanges();
                GenerateNodes();
            }
            else
            {
                var odgovor = MessageBox.Show("Kategorija sa ovim nazivom vec postoji!" +
               " Da li zelite da nastavite sa dodavanjem nove kategorije?", "Obavestenje", MessageBoxButton.YesNo);
                if (odgovor == MessageBoxResult.Yes)
                {
                    CategoryWindow();
                }
            }
           
            

        }
        public override void Dispose()
        {
            _ea.GetEvent<PassStringEventArgs>().Unsubscribe(CreateNewCategory);
            _ea.GetEvent<PassNewProductEventArgs>().Unsubscribe(CreateArticle);
            _ea.GetEvent<EditedProductEventArgs>().Unsubscribe(EditArticle);
            base.Dispose();
        }
    }
}
