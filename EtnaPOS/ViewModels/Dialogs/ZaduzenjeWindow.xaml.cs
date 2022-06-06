using System;
using System.Collections.Generic;
using System.Globalization;
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
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.Models;


namespace EtnaPOS.ViewModels.Dialogs
{
    /// <summary>
    /// Interaction logic for ZaduzenjeWindow.xaml
    /// </summary>
    /// 
    public partial class ZaduzenjeWindow : ThemedWindow
    {
        private EtnaDbContext _db => App.GetService<EtnaDbContext>();
        public ZaduzenjeWindow()
        {
            InitializeComponent();
            label.Content = _db.Documents.Where(s => s.Date == WorkDay.Date && s.IsOpen == false).Sum(d => (decimal?)d.TotalPrice).GetValueOrDefault();
        }
    }
}
