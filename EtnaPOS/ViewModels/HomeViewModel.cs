using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.Models;
using EtnaPOS.Services;
using EtnaPOS.ViewModels.Dialogs;
using Prism.Commands;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EtnaPOS.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private EtnaDbContext _db => App.GetService<EtnaDbContext>();
        public ICommand OpenDesignerCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ZaduzenjeCommand { get; }
        public ICommand PrintSettings { get; }
        public ICommand CloseDayCommand { get; }
        public ICommand DopunaCommand { get; }

        public HomeViewModel()
        {
            OpenDesignerCommand = new DelegateCommand(OpenDesigner);
            ImportCommand = new DevExpress.Mvvm.DelegateCommand(ImportArticles);
            ZaduzenjeCommand = new DevExpress.Mvvm.DelegateCommand(OpenZaduzenje);
            PrintSettings = new DevExpress.Mvvm.DelegateCommand(OpenSettings);
            CloseDayCommand = new DelegateCommand(CloseWorkDay);
            DopunaCommand = new DevExpress.Mvvm.DelegateCommand(PrintDopuna);
        }

        private void PrintDopuna()
        {
            var day = _db.ZatvaranjeDanas.Where(s => s.Date == WorkDay.Date)
                .Include(d => d.Documents.Select(g => g.Orders))
                .FirstOrDefault();
            if (day == null)
            {
                MessageBox.Show("ERROR 45 PRINT DOPUNA");
                return;
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
                MessageBox.Show("Error 93 CLOSE WORK DAY");
                return;
            }

            if (day.Documents.Any(c => c.IsOpen))
            {
                MessageBox.Show("Imate otvorene stolove");
                return;
            }

            day.IsClosed = true;
            _db.SaveChanges();

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

            try
            {
                PrintReceipt zad = new PrintReceipt(day);
                zad.PrintRazduzenje();

                Thread.Sleep(1000);

                PrintReceipt pr = new PrintReceipt(artikliZaDopunu);
                pr.PrintDopuna();

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
                List<Artikal> artikli = File.ReadAllLines(openFileDialog.FileName)
                                            .Skip(1)
                                            .Select(v => Artikal.FromCsv(v, id)).ToList();
                
                _db.Artikli.AddRange(artikli);
                _db.SaveChanges();
            }
        }

        private void OpenZaduzenje()
        {
            ZaduzenjeWindow window = new ZaduzenjeWindow();
            window.Show();
        }
        private void OpenDesigner()
        {
            SelectPrinterDialog printer = new SelectPrinterDialog();
            printer.ShowDialog();
        }
    }
}
