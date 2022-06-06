using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using EtnaPOS.Models;


namespace EtnaPOS.ViewModels.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectPrinterDialog.xaml
    /// </summary>
    public partial class SelectPrinterDialog : ThemedWindow
    {
        public SelectPrinterDialog()
        {
            InitializeComponent();
            InitializeComboBox();
            
        }

        private void InitializeComboBox()
        {
            List<string> printerNames = new List<string>();
            foreach (string installedPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printerNames.Add(installedPrinter);
            }
            comboBox.ItemsSource = printerNames;
            comboBox.SelectedIndex = 0;
            
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PrinterName = comboBox.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            DialogResult = true;
        }
    }
}
