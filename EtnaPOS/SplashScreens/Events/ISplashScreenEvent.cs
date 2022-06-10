namespace EtnaPOS.SplashScreens.Events;

public interface ISplashScreenEvent
{
    event SplashScreenEvent.fireSplashScreen OnSplashScreen;
    
    event SplashScreenEvent.stopSplashScreen OnStopSplashScreen;
    event SplashScreenEvent.fireSplashScreen OnTextChange;
    void StartSplashScreen(string text);
    void SplashScreenTextChange(string text);
    void StopSplashScreen();
}