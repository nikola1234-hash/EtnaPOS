using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EtnaPOS.Events.EventAggregator
{
    public class TablePositionEvent :PubSubEvent<TranslateTransform>
    {
    }
}
