using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.ViewModels.Dialogs;
using EtnaPOS.Windows;
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
        public HomeViewModel()
        {
            OpenDesignerCommand = new DelegateCommand(OpenDesigner);
            ImportCommand = new DevExpress.Mvvm.DelegateCommand(ImportArticles);
            ZaduzenjeCommand = new DevExpress.Mvvm.DelegateCommand(OpenZaduzenje);
            PrintSettings = new DevExpress.Mvvm.DelegateCommand(OpenSettings);
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
