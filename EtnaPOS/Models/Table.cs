using EtnaPOS.Behaviours;
using EtnaPOS.EtnaEventArgs;
using EtnaPOS.Events.EventAggregator;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EtnaPOS.Models
{
    public class Table : BaseModel
    {
        private IEventAggregator _ea => App.GetService<IEventAggregator>();
        
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


        DragBehavior dragBehavior { get; set; }
        public Table(string tableName)
        {
            Rectangle = new Rectangle();
            Id = Guid.NewGuid();
            TableName = tableName;
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(50, 205, 50));
            Rectangle.Width = 100;
            Rectangle.Height = 100;
            Top = 50;
            Left = 50;
            _ea.GetEvent<TablePositionEvent>().Subscribe(Instance_PositionChanged);
            var dbglistener = new TextWriterTraceListener(Console.Out);
            

        }

        private void Instance_PositionChanged(TranslateTransform e)
        {
            if(Top != e.Y)
            {
                Top = e.Y;
                OnPropertyChanged(nameof(Top));
            }
            if(Left != e.X)
            {
                Left = e.X;
                OnPropertyChanged(nameof(Left));
            }
            
            Debug.WriteLine("PositionChanged");
            Debug.WriteLine("Top: " + Top + " " + Left);
        }

       
        public override void Dispose()
        {
            _ea.GetEvent<TablePositionEvent>().Unsubscribe(Instance_PositionChanged);
            base.Dispose();
        }
    }
}
