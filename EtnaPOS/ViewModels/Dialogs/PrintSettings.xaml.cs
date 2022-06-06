using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EtnaPOS.DAL.Models;
using EtnaPOS.Services;

namespace EtnaPOS.ViewModels.Dialogs
{
    /// <summary>
    /// Interaction logic for PrintSettings.xaml
    /// </summary>
    public partial class PrintSettings : Window
    {
        public PrintSettings()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            fontSize.Text = Properties.Settings.Default.PrinterFontSize.ToString();
            layoutWidth.Text = Properties.Settings.Default.PrinterLayoutWidth.ToString();
            layoutHeight.Text = Properties.Settings.Default.PrinterLayoutHeight.ToString();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PrinterFontSize = int.Parse(fontSize.Text);
            Properties.Settings.Default.PrinterLayoutHeight = int.Parse(layoutHeight.Text);
            Properties.Settings.Default.PrinterLayoutWidth = int.Parse(layoutWidth.Text);
            Properties.Settings.Default.Save();
            DialogResult = true;
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            List<Order> oders = new List<Order>();
            Artikal art = new Artikal("Cola", 100, 4);
            Document doc = new Document()
            {
                Date = DateTime.Now,
                Orders = oders
            };
            PrintReceipt pr = new PrintReceipt(oders);
            pr.Blok();
            PrintReceipt pd = new PrintReceipt(doc);
            pd.Receipt();
           
        }
    }
}
