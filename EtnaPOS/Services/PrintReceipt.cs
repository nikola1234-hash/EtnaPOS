using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using EtnaPOS.DAL.Models;

namespace EtnaPOS.Services
{
    public class PrintReceipt : IDisposable
    {
        private Document Document { get; }

        private const int FIRST_COL_PAD = 20;
        private const int SECOND_COL_PAD = 7;
        private const int THIRD_COL_PAD = 20;

        public PrintReceipt(Document document)
        {
            if (document == null)
                throw new NullColumnChooserException();
            if (document.Orders == null || document.Orders.Count == 0)
                throw new Exception("There are no orders in document");
            Document = document;
        }


        private void CreateReceipt(object sender, PrintPageEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Racun");
            sb.AppendLine("==================");
            foreach (var order in Document.Orders.ToList())
            {
                sb.Append(order.Artikal.Name.PadRight(FIRST_COL_PAD));
                var breakDown = order.Count > 0 ? order.Count + "x" + order.Price : string.Empty;
                sb.Append(breakDown.PadRight(SECOND_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00}", order.Price).PadLeft(THIRD_COL_PAD));
            }
            sb.AppendLine("==================");
            var total = "Total: ";
            sb.Append(total.PadRight(SECOND_COL_PAD));
            sb.AppendLine(Document.TotalPrice.ToString().PadLeft(THIRD_COL_PAD));

            var printText = new PrintText(sb.ToString(), new Font("Monospace", 8));

            Graphics g = e.Graphics;
            var layoutArea = new SizeF(80, 0);
            SizeF stringSize = g.MeasureString(printText.Text, printText.Font, layoutArea, printText.StringFormat);

            RectangleF rectf = new RectangleF(new PointF(), new SizeF(80, stringSize.Height));

            g.DrawString(printText.Text, printText.Font, Brushes.Black, rectf, printText.StringFormat);

        }

        public void Receipt()
        {
            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateReceipt);
                doc.Print();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Blok()
        {

        }


        public void Dispose()
        {
            
        }
    }
}
