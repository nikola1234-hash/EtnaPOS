using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using EtnaPOS.DAL.Models;
using EtnaPOS.Models;

namespace EtnaPOS.Services
{
    public class PrintReceipt : IDisposable
    {
        private Document Document { get; }
        private List<Order> Orders { get; }
        private List<ArtikalDopuna> Dopuna { get; }

        private ZatvaranjeDana zd { get; }

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

        public PrintReceipt(List<ArtikalDopuna> lista)
        {
            if (lista == null)
            {
                Dopuna = new List<ArtikalDopuna>();
            }
            else
            {
                Dopuna = lista;
            }
            
        }

        public PrintReceipt(ZatvaranjeDana document)
        {
            zd = document;
        }

        private void CreateReceipt(object sender, PrintPageEventArgs e)
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
            string text = "RACUN";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            string date = DateTime.Now.ToString();
            e.Graphics.DrawString(date, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(date, drawFontArial12Bold).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;
            foreach (var order in Document.Orders)
            {
                e.Graphics.DrawString(order.Artikal.Name, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                y += e.Graphics.MeasureString(order.Artikal.Name, drawFontArial10Regular).Height;

                e.Graphics.DrawString("* " + order.Count, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString("* " + order.Count, drawFontArial10Regular).Height;

                e.Graphics.DrawString((order.Count * order.Artikal.Price).ToString(), drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString((order.Count * order.Artikal.Price).ToString(), drawFontArial10Regular).Height;
                
            }
            e.Graphics.DrawString("UKUPNO ZA UPLATU", drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
            y += e.Graphics.MeasureString("UKUPNO ZA UPLATU", drawFontArial10Regular).Height;
            e.Graphics.DrawString(Document.Orders.Sum(s=> s.Price).ToString(), drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatRight);
            y += e.Graphics.MeasureString(Document.Orders.Sum(s => s.Price).ToString(), drawFontArial10Regular).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;
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


       

        public void Blok()
        {
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
            string text = "PORUDZBINA";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            string date = DateTime.Now.ToString();
            e.Graphics.DrawString(date, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(date, drawFontArial12Bold).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;
            foreach (var documentOrder in Orders)
            {
                text = documentOrder.Artikal.Name;
                e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

                text = "* " + documentOrder.Count.ToString();
                e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

                text = (documentOrder.Count * documentOrder.Artikal.Price).ToString();
                e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;
            }

            
            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;
            

           
        }


    

        public void PrintDopuna()
        {
            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateDopuna);
                var psize = new PaperSize("racun", 100, 300000);

                doc.DefaultPageSettings.PaperSize = psize;
                doc.PrinterSettings.PrinterName = EtnaPOS.Models.PrinterSettings.PrinterName ?? throw new InvalidOperationException("No printer is chosen");
                doc.DefaultPageSettings.PaperSize.Height = 30000;
                doc.DefaultPageSettings.PaperSize.Width = 520;

                PrintDialog pd = new PrintDialog();
                pd.Document = doc;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.ShowDialog();

                doc.PrintPage -= new PrintPageEventHandler(CreateDopuna);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CreateDopuna(object sender, PrintPageEventArgs e)
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
            string text = "DOPUNA";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            string date = DateTime.Now.ToString();
            e.Graphics.DrawString(date, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(date, drawFontArial12Bold).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;

            if (!Dopuna.Any())
            {
                text = "NEMA DOPUNE";
                e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;
                return;
            }

            foreach (var artikal in Dopuna)
            {
                text = artikal.Name;
                e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

                text = "* " + artikal.Count;
                e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;
            }


            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;

        }

        public void PrintRazduzenje()
        {
            try
            {
                var doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler(CreateRazduzenje);
                var psize = new PaperSize("racun", 100, 300000);

                doc.DefaultPageSettings.PaperSize = psize;
                doc.PrinterSettings.PrinterName = EtnaPOS.Models.PrinterSettings.PrinterName ?? throw new InvalidOperationException("No printer is chosen");
                doc.DefaultPageSettings.PaperSize.Height = 30000;
                doc.DefaultPageSettings.PaperSize.Width = 520;

                PrintDialog pd = new PrintDialog();
                pd.Document = doc;
                pd.Document.DefaultPageSettings.PaperSize = psize;
                pd.ShowDialog();

                doc.PrintPage -= new PrintPageEventHandler(CreateRazduzenje);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void CreateRazduzenje(object sender, PrintPageEventArgs e)
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
            string text = "RAZDUZENJE";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            string date = DateTime.Now.ToString();
            e.Graphics.DrawString(date, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(date, drawFontArial12Bold).Height;

            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;

            decimal zaduzenje = zd.Documents.Sum(t=>t.TotalPrice);

            e.Graphics.DrawString(zaduzenje.ToString(), drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(zaduzenje.ToString(), drawFontArial12Bold).Height;


            e.Graphics.DrawString(lineBreak, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(lineBreak, drawFontArial12Bold).Height;
        }

        public void Dispose()
        {

        }
    }
}
