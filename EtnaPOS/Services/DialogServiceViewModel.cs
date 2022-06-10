using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.Models;
using EtnaPOS.ViewModels;

namespace EtnaPOS.Services
{
    public class DialogServiceViewModel : BaseViewModel
    {

        private DateTime _selectedDate = DateTime.Now.Date;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public List<UICommand> DialogCommands { get; private set; }
        protected UICommand CancelUiCommand { get; private set; }
        protected UICommand SelectUiCommand { get; private set; }

        private EtnaDbContext db => GetService<EtnaDbContext>();

        public DialogServiceViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DialogCommands = new List<UICommand>();
            CancelUiCommand = new UICommand(
                id: MessageBoxResult.Cancel,
                isCancel: true,
                isDefault: false,
                command: new DelegateCommand<CancelEventArgs>(CancelExecute),
                caption: "Odustani");

            SelectUiCommand = new UICommand(
                id: MessageBoxResult.OK,
                isDefault: true,
                isCancel: false,
                command: new DelegateCommand<CancelEventArgs>(ChoseDateExecute, CanChoseDateExecute),
                caption: "Izaberi"
            );

            DialogCommands.Add(SelectUiCommand);
            DialogCommands.Add(CancelUiCommand);
        }
        private void CancelExecute(CancelEventArgs args)
        {
            args.Cancel = false;
        }
        private bool CanChoseDateExecute(CancelEventArgs arg)
        {
            if (SelectedDate != DateTime.MinValue || SelectedDate != DateTime.MaxValue)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        private void ChoseDateExecute(CancelEventArgs obj)
        {
            if (SelectedDate != null)
            {
                try
                {

                    var db = App.GetService<EtnaDbContext>();
                    
                    WorkDay.Date = SelectedDate;

                    var otvaranjeDana = new ZatvaranjeDana(WorkDay.Date);
                    var day = db.ZatvaranjeDanas.FirstOrDefault(s => s.Date == WorkDay.Date);
                    if (db.ZatvaranjeDanas.Any(s => s.IsClosed == false && s.Date != WorkDay.Date))
                    {
                        var dan = db.ZatvaranjeDanas.FirstOrDefault(s => s.IsClosed == false && s.Date != WorkDay.Date);
                        MessageBox.Show("Datum :" + dan.Date.Date + " je i dalje otvoren");
                        obj.Cancel = true;

                    }
                    else if (day != null && day.IsClosed)
                    {
                     
                        MessageBox.Show("Ovaj dan je zakljucan ?", "Upozorenje");
                        obj.Cancel = true;
                    }

                    else if (day != null && day.IsClosed == false)
                    {
                        obj.Cancel = false;
                    }
                    else if (day == null)
                    {
                        db.ZatvaranjeDanas.Add(otvaranjeDana);
                        db.SaveChanges();
                        obj.Cancel = false;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
