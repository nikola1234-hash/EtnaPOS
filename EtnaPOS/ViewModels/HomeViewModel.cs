using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm.Native;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Models;
using EtnaPOS.Services;
using EtnaPOS.SplashScreens.Events;
using EtnaPOS.ViewModels.Dialogs;
using Microsoft.Extensions.Logging;
using Prism.Events;
using DelegateCommand = Prism.Commands.DelegateCommand;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EtnaPOS.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private EtnaDbContext _db => App.GetService<EtnaDbContext>();
        private IEventAggregator ea => App.GetService<IEventAggregator>();
        public ICommand OpenDesignerCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ZaduzenjeCommand { get; }
        public ICommand PrintSettings { get; }
        public ICommand CloseDayCommand { get; }
        public ICommand DopunaCommand { get; }
        public ICommand OtvoreniStoloviCommand { get; }
        public ICommand PromenaCenaCommand { get; }
        private ILogger<HomeViewModel> logger => App.GetService<ILogger<HomeViewModel>>();
        private ISplashScreenEvent splashScreen => App.GetService<ISplashScreenEvent>();
        public HomeViewModel()
        {
            OpenDesignerCommand = new DelegateCommand(OpenDesigner);
            ImportCommand = new DevExpress.Mvvm.DelegateCommand(ImportArticles);
            ZaduzenjeCommand = new DevExpress.Mvvm.DelegateCommand(OpenZaduzenje);
            PrintSettings = new DevExpress.Mvvm.DelegateCommand(OpenSettings);
            CloseDayCommand = new DelegateCommand(CloseWorkDay);
            DopunaCommand = new DevExpress.Mvvm.DelegateCommand(PrintDopuna);
            OtvoreniStoloviCommand = new DevExpress.Mvvm.DelegateCommand(OtvoreniStolovi);
            PromenaCenaCommand = new DevExpress.Mvvm.DelegateCommand(PriceChangeByCsv);

        }

        private void OtvoreniStolovi()
        {
            OtvoreniStoloviWindow window = new OtvoreniStoloviWindow();
            window.ShowDialog();
        }

        private void PrintDopuna()
        {
            var documents = _db.Documents.Where(s => s.Date == WorkDay.Date)
                .Include(d => d.Orders);
            if (documents == null || !documents.Any())
            {
                MessageBox.Show("ERROR 45 PRINT DOPUNA, NEMA DOKUMENATA");
                logger.LogError("Print error, nema dokumenta");
                return;
            }

            List<ArtikalDopuna> artikliZaDopunu = new List<ArtikalDopuna>();
            foreach (var dopuna in documents)
            {
                foreach (var order in dopuna.Orders)
                {
                    var artikal = _db.Artikli.Find(order.ArtikalId);
                    var a = artikliZaDopunu.FirstOrDefault(s => s.Name == artikal.Name);
                    if (a != null)
                    {
                        a.Count += order.Count;
                    }
                    else
                    {
                        artikliZaDopunu.Add(new ArtikalDopuna(artikal.Name, order.Count));
                    }
                }
            }

            try
            {

                PrintReceipt pr = new PrintReceipt(artikliZaDopunu);
                pr.PrintDopuna();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska");
            }

        }

        private void CloseWorkDay()
        {
            var result = MessageBox.Show("Da li zelite da zatvorite ovaj radni dan ?", "Upozorenje",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            var day = _db.ZatvaranjeDanas.Where(s => s.Date == WorkDay.Date)
                .Include(d => d.Documents.Select(g => g.Orders))
                .FirstOrDefault();
            if (day == null)
            {
                MessageBox.Show("Error 109 CLOSE WORK DAY");
                return;
            }
            try 
            {
                IEnumerable<Document> documents = new List<Document>();
                bool hasDocuments = false;
                if (day.Documents.Any(c => c.IsOpen))
                {
                    var dialogResult = MessageBox.Show("Imate otvorene stolove, da li zelite da ih prebacite u novi radni dan?"
                                    , "Obavestenje", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        documents = day.Documents.Where(c => c.IsOpen).ToList();

                        foreach (var document in documents)
                        {
                            day.Documents.Remove(document);
                        }

                        hasDocuments = true;
                        day.IsClosed = true;
                        _db.SaveChanges();
                        ea.GetEvent<ChoseWorkingDayEventAggregator>().Publish();

                        var newDay = _db.ZatvaranjeDanas.FirstOrDefault(s => s.IsClosed == false);

                        if(newDay.Documents == null) newDay.Documents = new List<Document>();

                        foreach (var document in documents)
                        {
                            document.Date = WorkDay.Date;
                            newDay.Documents.Add(document);
                        }

                        _db.SaveChanges();

                        MessageBox.Show("Uspesno prebaceni dokumenti u novi dan");
                    }
                    else
                    {
                        return;
                    }
                }


                if (!hasDocuments)
                {
                    day.IsClosed = true;

                    _db.SaveChanges();
                }
                List<ArtikalDopuna> artikliZaDopunu = new List<ArtikalDopuna>();
                foreach (var dopuna in day.Documents)
                {
                    foreach (var order in dopuna.Orders)
                    {
                        var a = artikliZaDopunu.FirstOrDefault(s => s.Name == order.Artikal.Name);
                        if (a != null)
                        {
                            a.Count += order.Count;
                        }
                        else
                        {
                            artikliZaDopunu.Add(new ArtikalDopuna(order.Artikal.Name, order.Count));
                        }
                    }
                }

                PrintReceipt zad = new PrintReceipt(day);
                zad.PrintRazduzenje();



                PrintReceipt pr = new PrintReceipt(artikliZaDopunu);
                pr.PrintDopuna();


                if (!hasDocuments)
                {
                    ea.GetEvent<ChoseWorkingDayEventAggregator>().Publish();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Greska");
            }

        }

        private void OpenSettings()
        {
            Dialogs.PrintSettings ps = new PrintSettings();
            ps.ShowDialog();
        }

        private void ImportArticles()
        {
          
            
            int id = 0;
            var doesExist = _db.Kategorije.FirstOrDefault(n => n.Kategorija == "Svi Artikli");
            if (doesExist == null)
            {
                var k = new KategorijaArtikla()
                {
                    Kategorija = "Svi Artikli",
                    CreatedBy = "System",
                    DateCreated = DateTime.Now,
                    ParentId = 1
                };
                var entity  = _db.Kategorije.Add(k);
                _db.SaveChanges();
                id = entity.Id;
            }
            else
            {
                id = doesExist.Id;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                splashScreen.StartSplashScreen("Ubacivanje artikala");
                try
                {
                    splashScreen.SplashScreenTextChange("Mapiranje artikala");
                    List<Artikal> artikli = File.ReadAllLines(openFileDialog.FileName)
                                            .Skip(1)
                                            .Select(v => Artikal.FromCsv(v, id)).ToList();
                    splashScreen.SplashScreenTextChange("Dodavanje u bazu.");
                    _db.Artikli.AddRange(artikli);
                    splashScreen.SplashScreenTextChange("Cuvanje");
                    _db.SaveChanges();
                    splashScreen.SplashScreenTextChange("Uspesno!");
                }
                catch(Exception ex)
                {
                    logger.LogCritical("Dodavanje artikla ima gresku: " + ex.Message);

                    splashScreen.SplashScreenTextChange("GRESKA!");

                    MessageBox.Show("Dodavanje artikala ima gresku: " + ex.Message);
                }
                splashScreen.StopSplashScreen();
            }
        }
        public void PriceChangeByCsv()
        {
            int id = 0;
            var doesExist = _db.Kategorije.FirstOrDefault(n => n.Kategorija == "Svi Artikli");
            if (doesExist == null)
            {
                var k = new KategorijaArtikla()
                {
                    Kategorija = "Svi Artikli",
                    CreatedBy = "System",
                    DateCreated = DateTime.Now,
                    ParentId = 1
                };
                var entity = _db.Kategorije.Add(k);
                _db.SaveChanges();
                id = entity.Id;
            }
            else
            {
                id = doesExist.Id;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                splashScreen.StartSplashScreen("Menjanje cena");
                try
                {
                    splashScreen.SplashScreenTextChange("Mapiranje artikala");
                    List<Artikal> artikli = File.ReadAllLines(openFileDialog.FileName)
                                            .Skip(1)
                                            .Select(v => Artikal.FromCsv(v, id)).ToList();

                    splashScreen.SplashScreenTextChange("Promena cena...");
                    var artikliDb = _db.Artikli.ToList();
                    foreach(var artikl in artikli)
                    {
                        foreach(var artiklDb in artikliDb)
                        {
                            if(artiklDb.Name == artikl.Name)
                            {
                                if(artiklDb.Price != artikl.Price)
                                {
                                    artiklDb.Price = artikl.Price;
                                }
                            }
                        }
                    }


                    splashScreen.SplashScreenTextChange("Cuvanje");
                    _db.SaveChanges();
                    splashScreen.SplashScreenTextChange("Uspesno!");
                }
                catch (Exception ex)
                {
                    logger.LogCritical("Promena cene ima gresku: " + ex.Message);

                    splashScreen.SplashScreenTextChange("GRESKA!");

                    MessageBox.Show("Promena cene ima gresku: " + ex.Message);
                }
                splashScreen.StopSplashScreen();
            }
        }
        private void OpenZaduzenje()
        {
            ZaduzenjeWindow window = new ZaduzenjeWindow();
            window.ShowDialog();
            
        }
        private void OpenDesigner()
        {
            SelectPrinterDialog printer = new SelectPrinterDialog();
            printer.ShowDialog();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
