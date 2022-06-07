using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraReports.UI;
using EtnaPOS.DAL.Models;
using EtnaPOS.Windows;

namespace EtnaPOS.Services
{
    public class PrintReceipt : IDisposable
    {
        private Document Document { get; }
        private List<Order> Orders { get; }

        private const int FIRST_COL_PAD = 20;
        private const int SECOND_COL_PAD = 7;
        private const int THIRD_COL_PAD = 20;
        private int fontSize => Properties.Settings.Default.PrinterFontSize;
        private int layoutWidth => Properties.Settings.Default.PrinterLayoutWidth;
        private int layoutHeight => Properties.Settings.Default.PrinterLayoutHeight;
        public PrintReceipt(Document document)
        {
            if (document == null)
                throw new NullColumnChooserException();
            if (document.Orders == null || document.Orders.Count == 0)
                throw new Exception("There are no orders in document");
            Document = document;
        }

        public PrintReceipt(List<Order> orders)
        {
            if (orders == null)
                throw new NullReferenceException();
            if (orders.Count == 0)
            {
                return;
            }

            Orders = orders;

        }

        private void CreateReceipt(object sender, PrintPageEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Racun");
            sb.AppendLine("Vreme: " + DateTime.Now);
            sb.AppendLine("===========================");
            foreach (var order in Document.Orders.ToList())
            {
                sb.Append(order.Artikal.Name.PadRight(FIRST_COL_PAD));
                var breakDown = order.Count > 0 ? order.Count + "* " + order.Price : string.Empty;
                sb.Append(breakDown.PadRight(SECOND_COL_PAD));
                sb.AppendLine(string.Format("{0:0.00}", order.Price * order.Count).PadLeft(THIRD_COL_PAD));
            }
            sb.AppendLine("===========================");
            var total = "Total: ";
            sb.Append(total.PadRight(SECOND_COL_PAD));
            sb.AppendLine(Document.Orders.Sum(s=> s.Price).ToString().PadLeft(THIRD_COL_PAD));
           

            var printText = new PrintText(sb.ToString(), new Font("Monospace", fontSize));
            
            Graphics g = e.Graphics;
            var layoutArea = new SizeF(layoutWidth, layoutHeight);
            SizeF stringSize = g.MeasureString(printText.Text, printText.Font, layoutArea, printText.StringFormat);

            RectangleF rectf = new RectangleF(new PointF(), new SizeF(layoutWidth, stringSize.Height));

            g.DrawString(printText.Text, printText.Font, Brushes.Black, rectf, printText.StringFormat);

        }

        public void Receipt()
        {
            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateReceipt);
                var psize = new PaperSize("racun", 100, 300000);
                doc.DefaultPageSettings.PaperSize = psize;
                doc.PrinterSettings.PrinterName = EtnaPOS.Models.PrinterSettings.PrinterName ?? throw new InvalidOperationException("No printer is chosen");
                doc.DefaultPageSettings.PaperSize.Height = 30000;
                doc.DefaultPageSettings.PaperSize.Width = 520;
                PrintDialog pd = new PrintDialog();
                pd.Document = doc;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.ShowDialog();
                if (doc.PrinterSettings.IsValid)
                {
                    
                    doc.Print();
                }
                doc.PrintPage -= new PrintPageEventHandler(CreateReceipt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void PrintBlok(object sender, PrintPageEventArgs e)
        {
            try
            {
                //-----------
                Graphics graphics = e.Graphics;
                Font font = new Font("calibri", 10);
                float fontHeight = font.GetHeight();
                String underLine = "-------------------------------------------------------------";
                int startX = 10;
                int startY = 20;
                int Offset = 10;
                Offset += 0;
                Offset = Offset + 15;
                graphics.DrawString(underLine, new Font("calibri", 10), new SolidBrush(Color.Black), 0, startY + Offset);
                Offset = Offset + 15;
                graphics.DrawString("Daily Deployment For Date " + DateTime.Now.ToString("d, MMMM, yyyy"), new Font("Calibri", 10), new SolidBrush(Color.Black), startX + 15, startY + Offset);
                Offset = Offset + 15;
                graphics.DrawString(underLine, new Font("calibri", 10), new SolidBrush(Color.Black), 0, startY + Offset);
                double tAmount = 0;
                double gTotal = 0;
                string xParent = "*";
                bool isHeader = true;
                for (int i = 0; i < Document.Orders.Count; i++)
                {
                    var parent = Document.Orders.ElementAt(i);
                    if (isHeader)
                    {
                        tAmount = 0;
                        Offset = Offset + 15;
                        graphics.DrawString("RACUN", new Font("Calibri", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                        Offset = Offset + 15;
                        graphics.DrawString(underLine, new Font("calibri", 10), new SolidBrush(Color.Black), 0, startY + Offset);
                        Offset = Offset + 10;
                        graphics.DrawString("Artikal                                                   Kolicina", new Font("Calibri", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                        Offset = Offset + 10;
                        graphics.DrawString(underLine, new Font("calibri", 10), new SolidBrush(Color.Black), 0, startY + Offset);
                        Offset = Offset + 15;
                        isHeader = false;
                    }
                    string item = (i + 1) + " - " + parent.Artikal.Name;
                    string quantity = parent.Count.ToString();
                    graphics.DrawString(item, new Font("Calibri", 8), new SolidBrush(Color.Black), startX, startY + Offset);
                    graphics.DrawString(quantity, new Font("Calibri", 8), new SolidBrush(Color.Black), startX + 180, startY + Offset);
                    Offset = Offset + 15;
                    //string nextParent;
                    //if (i < wStock.Rows.Count - 1)
                    //    nextParent = wStock.Rows[i + 1]["toLocation"].ToString();
                    //else
                    //    nextParent = "*";
                }
                graphics.DrawString(underLine, new Font("calibri", 10), new SolidBrush(Color.Black), 0, startY + Offset);
                Offset = Offset + 15;
                graphics.DrawString("Print Time :  " + DateTime.Now.ToString("d, MMMM, yyyy. hh:mm - tt "), new Font("Calibri", 10), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 15;

                graphics.DrawString("By : www.codemodes.com", new Font("Calibri", 10), new SolidBrush(Color.Black), startX, startY + Offset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Blok()
        {
           
            

            //var report = new XtraReport();
            //report.RollPaper = true;
            //report.ReportUnit = ReportUnit.HundredthsOfAnInch;
            //report.PaperKind = PaperKind.Custom;
            //report.PageWidth = 312;
            //report.Margins = new Margins(0, 0, 0, 0);

            //var printTool = new ReportPrintToolWpf(report);
            //printTool.PrintDialog();
            //printTool.Print();


            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateBlok);
                var psize = new PaperSize("racun", 100, 300000);
                doc.DefaultPageSettings.PaperSize = psize;
                doc.PrinterSettings.PrinterName = EtnaPOS.Models.PrinterSettings.PrinterName ?? throw new InvalidOperationException("No printer is chosen");
                doc.DefaultPageSettings.PaperSize.Height = 30000;
                doc.DefaultPageSettings.PaperSize.Width = 520;
                PrintDialog pd = new PrintDialog();
                pd.Document = doc;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.ShowDialog();
                if (doc.PrinterSettings.IsValid)
                {
                    
                    doc.Print();
                }
                doc.PrintPage -= new PrintPageEventHandler(CreateBlok);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateBlok(object sender, PrintPageEventArgs e)
        {

            float x = 10;
            float y = 5;
            float width = 270.0F; // max width I found through trial and error
            float height = 0F;

            Font drawFontArial12Bold = new Font("Arial", 12, FontStyle.Bold);
            Font drawFontArial10Regular = new Font("Arial", 10, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            string lineBreak = "==========================";
            // Draw string to screen.
            string text = "Porudzbina";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            string date = DateTime.Now.ToString();
            e.Graphics.DrawString(date, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;
            foreach (var documentOrder in Orders)
            {
                e.Graphics.DrawString(documentOrder.Artikal.Name, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;
                e.Graphics.DrawString("* " + documentOrder.Count.ToString(), drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;
                e.Graphics.DrawString((documentOrder.Count * documentOrder.Artikal.Price).ToString(), drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;
            }

            
            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;
            

           
        }


        public void Dispose()
        {
            
        }
    }
}
