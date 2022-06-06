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
using EtnaPOS.Models;
using MessageBox = System.Windows.MessageBox;


namespace EtnaPOS.Windows
{
    /// <summary>
    /// Interaction logic for SelectWorkDay.xaml
    /// </summary>
    public partial class SelectWorkDay : ThemedWindow
    {
        public SelectWorkDay()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                if (datePicker.SelectedDate == DateTime.Now.Date)
                {
                    WorkDay.Date = (DateTime)datePicker.SelectedDate.Value;
                    DialogResult = true;
                    

                }
                if (datePicker.SelectedDate > DateTime.Now.Date)
                {
                    var result = MessageBox.Show("Da li stvarno zelite da predjete u novi datum ?", "Upozorenje",
                        MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        WorkDay.Date = (DateTime)datePicker.SelectedDate.Value;
                        DialogResult = true;
                    }
                    else
                    {
                        DialogResult = false;
                    }

                }
                if (datePicker.SelectedDate < DateTime.Now.Date)
                {
                    var result = MessageBox.Show("Da li stvarno zelite da vatite datum ?", "Upozorenje",
                        MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        WorkDay.Date = (DateTime)datePicker.SelectedDate.Value;
                        DialogResult = true;
                    }
                    else
                    {
                        DialogResult = false;
                    }

                }
            }
        }
    }
}
