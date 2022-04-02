using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EtnaPOS.Models
{
    public class Table : BaseModel
    {
        public Guid Id { get; set; }
        private string _tableName;

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                OnPropertyChanged();
            }
        }
        private double _top;

        public double Top
        {
            get { return _top; }
            set
            { 
                _top = value;
                OnPropertyChanged();
            }
        }

        private double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }

        private Rectangle _rectangle;

        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set 
            { 
                _rectangle = value;
                OnPropertyChanged(nameof(Rectangle));
            }
        }



        public Table(string tableName)
        {
            Rectangle = new Rectangle();
            Id = Guid.NewGuid();
            TableName = tableName;
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(25, 25, 25));
            Rectangle.Width = 50;
            Rectangle.Height = 50;
            Top = 50;
            Left = 50;

        }
    }
}
