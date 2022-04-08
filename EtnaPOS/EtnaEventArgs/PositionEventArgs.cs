using System;

namespace EtnaPOS.EtnaEventArgs
{
    public class PositionEventArgs : EventArgs
    {
        public double X { get; }
        public double Y { get; }
        public PositionEventArgs(double x, double y)
        {
            Y = y;
            X = x;
        }
    }
}
