using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtnaPOS.SplashScreens.Events
{
    public class SplashScreenEvent : ISplashScreenEvent
    {
        public delegate void fireSplashScreen(object source, SplashEventArgs e);
        public event fireSplashScreen OnSplashScreen;

        public delegate void stopSplashScreen(object source, EventArgs e);
        public event stopSplashScreen OnStopSplashScreen;

        public event fireSplashScreen OnTextChange;
        public void StartSplashScreen(string text)
        {
            if (OnSplashScreen != null)
            {
                OnSplashScreen.Invoke(this, new SplashEventArgs(text));
            }
        }

        public void StopSplashScreen()
        {
            if (OnStopSplashScreen != null)
            {
                OnStopSplashScreen.Invoke(this, new EventArgs());
            }
        }

        public void SplashScreenTextChange(string text)
        {
            if (OnTextChange != null)
            {
                OnTextChange.Invoke(this, new SplashEventArgs(text));
            }
        }
    }
}
