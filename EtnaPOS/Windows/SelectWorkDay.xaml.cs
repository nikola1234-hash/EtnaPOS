using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;
using EtnaPOS.Models;
using MessageBox = System.Windows.MessageBox;


namespace EtnaPOS.Windows
{
    /// <summary>
    /// Interaction logic for SelectWorkDay.xaml
    /// </summary>
    /// 
    public partial class SelectWorkDay : ThemedWindow
    {
        public SelectWorkDay()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now.Date;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

            if (datePicker.SelectedDate != null)
            {
                var db = App.GetService<EtnaDbContext>();
                if (datePicker.SelectedDate == DateTime.Now.Date)
                {
                    WorkDay.Date = datePicker.SelectedDate.Value;
                    var otvaranjeDana = new ZatvaranjeDana(WorkDay.Date);
                    var day = db.ZatvaranjeDanas.FirstOrDefault(s=> s.Date == WorkDay.Date);
                    if (day != null && day.IsClosed)
                    {
                        MessageBox.Show("Ovaj dan je zakljucan ?", "Upozorenje");
                    }
                    if (day != null && day.IsClosed == false)
                    {
                        DialogResult = true;
                    }
                    else if(day == null)
                    {
                        db.ZatvaranjeDanas.Add(otvaranjeDana);
                        db.SaveChanges();
                        DialogResult = true;
                    }
                }
           
            }
        }
    }
}
