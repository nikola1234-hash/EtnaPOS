using System;
using System.Collections.Generic;
using EtnaPOS.DAL.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;
using EtnaPOS.Services;

namespace EtnaPOS.ViewModels
{
    public class KasaViewModel : BaseViewModel
    {
        private int tableId { get; }
        private Document dbDocument { get; set; }
        public string TableNumber { get; }
        private EtnaDbContext db => App.GetService<EtnaDbContext>()!;
        private ObservableCollection<ArtikalKorpaViewModel> _korpa;

        public ObservableCollection<ArtikalKorpaViewModel> Korpa
        {
            get { return _korpa; }
            set
            {
                _korpa = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Artikal> _artikli;

        public ObservableCollection<Artikal> Artikli
        {
            get { return _artikli; }
            set
            {
                _artikli = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Artikal> _artikliCache;

        public ObservableCollection<Artikal> ArtiklCache
        {
            get { return _artikliCache; }
            set
            {
                _artikliCache = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    Artikli = ArtiklCache.Where(s => s.Name.ToLower().Contains(value.ToLower()))
                        .ToObservableCollection();

                    OnPropertyChanged(nameof(Artikli));
                    OnPropertyChanged(nameof(SearchText));
                }

            }
        }

        private bool _canDelete;

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {

                _canDelete = value;
                OnPropertyChanged();
            }
        }

        private bool _canCloseOrder;

        public bool CanCloseOrder
        {
            get { return _canCloseOrder; }
            set
            {
                _canCloseOrder = value;
                OnPropertyChanged(nameof(CanCloseOrder));
            }
        }

        private ICurrentWindowService _currentWindowService => GetService<ICurrentWindowService>();
        public ICommand DoubleClickCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand CheckOutCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand CloseOrderCommand { get; }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value is ArtikalKorpaViewModel)
                {
                    CanDelete = true;
                }
                else
                {
                    CanDelete = false;
                }

                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private decimal _totalPrice;

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

       
        public KasaViewModel(int tableId)
        {
            this.tableId = tableId;

            TableNumber = "Sto: " + db.Tables.Find(tableId)!.TableName;
            dbDocument = db.Documents.FirstOrDefault(s => s.TableId == tableId && s.Date == WorkDay.Date && s.IsOpen) ?? null;

            DoubleClickCommand = new DelegateCommand(AddArtikalToList);
            RemoveCommand = new DelegateCommand(RemoveArtikalFromList);

            CheckOutCommand = new DelegateCommand(OrderCheckOut);
            ExitCommand = new DelegateCommand(CloseWindow);

            CloseOrderCommand = new DelegateCommand(CloseOrder);

            Korpa = new ObservableCollection<ArtikalKorpaViewModel>();
            Korpa.CollectionChanged += Korpa_CollectionChanged;

            InitializeBasket(tableId);

            InitializeArticles();

        }

        private void InitializeBasket(int tableId)
        {
            var document = db.Documents.Where(s => s.TableId == tableId && s.IsOpen && s.Date == WorkDay.Date).Include(d => d.Orders).FirstOrDefault();
          
            if (document != null)
            {
                foreach (var order in document.Orders)
                {
                    var artikal = db.Artikli.Find(order.ArtikalId);
                    Korpa.Add(new ArtikalKorpaViewModel(artikal, order.Count));
                }
                CalculateTotalPrice();
            }
        }

        private void Korpa_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Korpa.Count > 0 && dbDocument != null)
            {
                
                CanCloseOrder = true;
                OnPropertyChanged(nameof(CanCloseOrder));
            }
            else
            {
                CanCloseOrder = false;
                OnPropertyChanged(nameof(CanCloseOrder));
            }
        }

        private void CloseWindow()
        {
            _currentWindowService.Close();
        }

        private void OrderCheckOut()
        {
            ///Ako udjemo i izadjemo sa stola be kucanja.
            if (dbDocument == null && Korpa.Count == 0)
            {
                CloseWindow();
                return;
            }


            List<Order> printOrders = new List<Order>();

            if (Korpa.Count == 0)
            {
                dbDocument!.IsOpen = false;
                db.SaveChanges();
            }
            if (Korpa.Count > 0)
            {
               
                if (dbDocument != null)
                {
                    foreach (var artikal in Korpa)
                    {
                        
                        foreach (var documentOrder in dbDocument.Orders.ToList())
                        {
                            if (documentOrder.ArtikalId == artikal.Artikal.Id)
                            {
                                if (documentOrder.Count != artikal.Count)
                                {
                                    documentOrder.Count = artikal.Count;
                                    
                                }
                              
                            }
                            else
                            {
                                var order = db.Orders.Create();
                                order.Artikal = artikal.Artikal;
                                order.ArtikalId = artikal.Artikal.Id;
                                order.Count = artikal.Count;
                                order.Date = DateTime.Now.Date;
                                order.Id = Guid.NewGuid();
                                order.Time = DateTime.Now;
                                order.IsDeleted = false;
                                order.Price = artikal.Artikal.Price * artikal.Count;


                                dbDocument.Orders.Add(order);
                                
                                printOrders.Add(order);
                                
                            }
                        }
                    }


                    PrintReceipt printeReceipt = new PrintReceipt(printOrders);
                    printeReceipt.Blok();

                }
                else
                {
                    ICollection<Order> orders = new List<Order>();
                    foreach (var artikal in Korpa)
                    {
                        orders.Add(new Order()
                        {
                            ArtikalId = artikal.Artikal.Id,
                            Count = artikal.Count,
                            Date = DateTime.Now.Date,
                            Id = Guid.NewGuid(),
                            Time = DateTime.Now,
                            IsDeleted = false,
                            Price = artikal.Artikal.Price * artikal.Count,
                            Artikal = artikal.Artikal
                        });
                    }
                    // Print slip 
                    Document doc = new Document()
                    {
                        TableId = tableId,
                        Orders = orders,
                        Date = WorkDay.Date,
                        Time = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IsOpen = true,
                        TotalPrice = 0,
                    };
                    dbDocument = db.Documents.Add(doc);

                    var dan = db.ZatvaranjeDanas.FirstOrDefault(s => s.Date == WorkDay.Date);
                    if (dan.Documents == null)
                    {
                        dan.Documents = new List<Document>();
                    }
                    dan.Documents.Add(dbDocument);

                    PrintReceipt printReceipt = new PrintReceipt(orders.ToList());
                    printReceipt.Blok();
                }


                try
                {

                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Greska prilikom cuvanja dokumenta");
                }


                // Create The document
                // Close the window
            }
            else
            {
                CloseWindow();
            }

            CloseWindow();
        }

        private void CloseOrder()
        {
            
            if (dbDocument != null)
            {
                dbDocument.TotalPrice = dbDocument.Orders.Sum(s => s.Price);
                dbDocument.IsOpen = false;
                var eState = db.SaveChanges();
                if (eState > 0)
                {
                    PrintReceipt printReceipt = new PrintReceipt(dbDocument);
                    printReceipt.Receipt();
                    CloseWindow();
                    // print slip
                }

            }
            // Close Order Document
            // print slip
            // close window
        }
        private void CalculateTotalPrice()
        {
            TotalPrice = Korpa.Sum(x => x.TotalPrice);
        }
        protected void AddArtikalToList()
        {
            if(SelectedItem is Artikal artikal)
            {
                if (Korpa.FirstOrDefault(s=>s.Artikal.Id == artikal.Id) != null)
                {
                    Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Id)!.Count += 1;
                    CalculateTotalPrice();
                }
                else
                {
                    Korpa.Add(new ArtikalKorpaViewModel(artikal, 1));
                    CalculateTotalPrice();

                }
            }
        }

        

        protected void RemoveArtikalFromList()
        {
            if (SelectedItem is ArtikalKorpaViewModel artikal)
            {
                if (Korpa.Contains((artikal)))
                {
                    if (Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count > 1)
                    {
                        Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count -= 1;
                        CalculateTotalPrice();
                    }
                    else if(Korpa.FirstOrDefault(s => s.Artikal.Id == artikal.Artikal.Id)!.Count == 1)
                    {
                        Korpa.Remove(artikal);

                        var document = db.Documents.Where(s =>
                            s.TableId == tableId && s.IsOpen).Include(s => s.Orders).FirstOrDefault();
                        if (document != null)
                        {
                            var order = document.Orders.FirstOrDefault(s => s.ArtikalId == artikal.Artikal.Id);
                            if (order != null)
                            {
                                document.Orders.Remove(order);
                                order.IsDeleted = true;
                                db.SaveChanges();
                            }
                            
                        }
                        CalculateTotalPrice();
                    }
                }
            }
        }
        protected void InitializeArticles()
        {
            if (Artikli == null)
            {
                Artikli = new ObservableCollection<Artikal>();
                ArtiklCache = new ObservableCollection<Artikal>();
            }
            else if (Artikli.Count > 0)
            {
                Artikli.Clear();
                ArtiklCache.Clear();
            }
            Artikli = db.Artikli.Where(s => s.IsActive).ToObservableCollection();
            ArtiklCache = Artikli;
        }

        public override void Dispose()
        {
            base.Dispose();
            
        }
    }
}
