using System;

namespace EtnaPOS.SplashScreens.Events
{
    public class SplashEventArgs : EventArgs
    {
        public string Text { get; }

        public SplashEventArgs(string text)
        {
            Text = text;
        }
    }
}
