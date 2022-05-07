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

        public string TableName { get; set; }
        public int Id { get; set; }
        private SolidColorBrush _buttonBackground;

        public SolidColorBrush ButtonBackground
        {
            get { return _buttonBackground; }
            set
            {
                _buttonBackground = value;
                OnPropertyChanged();
            }
        }
        public Table(string tableName)
        {
            TableName = tableName;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
