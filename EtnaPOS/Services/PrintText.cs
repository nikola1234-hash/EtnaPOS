using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtnaPOS.Services
{
    public class PrintText
    {
        public PrintText(string text, Font font) : this(text, font, new StringFormat()){}

        public PrintText(string text, Font font, StringFormat format)
        {
            Text = text;
            Font = font;
            StringFormat = format;
        }
        public string Text { get; set; }
        public Font Font { get; set; }

        public StringFormat StringFormat
        {
            get;
            set;
        }
    }
}
