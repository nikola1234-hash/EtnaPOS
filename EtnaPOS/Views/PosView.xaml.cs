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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EtnaPOS.Views
{
    /// <summary>
    /// Interaction logic for PosView.xaml
    /// </summary>
    public partial class PosView : UserControl
    {
        public PosView()
        {
            InitializeComponent();
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.Serializable);
            if(data is UIElement element)
            {
                var dropPosition = e.GetPosition(element);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);
                
               
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
