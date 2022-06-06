using System;

namespace EtnaPOS.Models
{
    public static class PrinterSettings
    {
        public static string PrinterName
        {
            get
            {
                if (Properties.Settings.Default.PrinterName != null) return Properties.Settings.Default.PrinterName;
                else
                {
                    throw new Exception("Printer not chosen");
                }
            }
        }
    }
}
