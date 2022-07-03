using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Models;
using Prism.Events;

namespace EtnaPOS.ViewModels.WindowViewModels
{
    public class ChangeTableViewModel : BaseViewModel
    {
        private EtnaDbContext db = App.GetService<EtnaDbContext>();
        private IEventAggregator _eventAggregator = App.GetService<IEventAggregator>();

        private ICurrentWindowService window => GetService<ICurrentWindowService>();

        private ObservableCollection<Table> _tables;

        public ObservableCollection<Table> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                OnPropertyChanged();
            }
        }

        public ICommand<int> SelectTableCommand { get; }
        private void LoadTables()
        {
            if (Tables != null)
            {
                Tables.Clear();
            }
            var tables = db.Tables.ToList();
            if (tables != null)
            {
                Tables = new ObservableCollection<Table>();
                var openTable = db.Documents.Where(s => s.IsOpen == true).Select(d => d.TableId);
                foreach (var table in tables)
                {
                    if (!openTable.Contains(table.Id))
                    {
                        Tables.Add(new Table(table));
                    }
                   
                }
            }
            OnPropertyChanged(nameof(Tables));
        }

        public ChangeTableViewModel()
        {
            LoadTables();
            SelectTableCommand = new DelegateCommand<int>(SelectTable);
        }

        private void SelectTable(int id)
        {
            _eventAggregator.GetEvent<ChangeTableEventAggregator>().Publish(id);
            MessageBox.Show("Uspesno promenjen sto");
            window.Close();
        }


        public override void Dispose()
        {
           
            base.Dispose();
        }
    }
}
